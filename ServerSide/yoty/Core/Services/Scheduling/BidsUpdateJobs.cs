// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Services.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using YOTY.Service.Core.Managers.Bids;
    using YOTY.Service.Core.Managers.Notifications;
    using YOTY.Service.Core.Services.Mail;
    using YOTY.Service.Data;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class BidsUpdateJobs
    {
        
        private  static MailSettings MailSettings;
        private  static MailSecrets MailSecrets;
        private  static IServiceScopeFactory ScopeFactory;

        public BidsUpdateJobs(IServiceScopeFactory scopeFactory, IOptions<MailSettings> mailSettings , IOptions<MailSecrets> mailSecrets)
        {
            MailSettings = mailSettings.Value;
            MailSecrets = mailSecrets.Value;
            ScopeFactory = scopeFactory;
        }
        
        
        public async Task UpdateBidsPhaseDaily()
        {
            IMailService mail = new MailService(MailSettings, MailSecrets);

            /*IMailService mail = new MailService(
                new MailSettings() { DisplayName = "UniBuy", Host = "smtp.gmail.com", Mail = "unibuy.notifications@gmail.com", Port = 587 },
                new MailSecrets() { Password = "UniBuyIsTheBest" });
                */
            //YotyContext context = new YotyContext();
            // don't need mapper rn

            using (var scope = ScopeFactory.CreateScope())
            {
                YotyContext context  = scope.ServiceProvider.GetRequiredService<YotyContext>();
                try
                {

                    var ids = context.Bids
                        // צחי האופטימיזציה הזאת שלך לא נתמכת ע"י הקונטקס בנתיים אז זה תמיד החזיר רשימה ריקה
                        // זיינת אותי
                        // .Where(bid => bid.Phase<BidPhase.CancelledSupplierNotFound && bid.ExpirationDate < DateTime.UtcNow.AddHours(2))
                        .Select(bid => bid.Id)
                        .ToList();

                    foreach (var id in ids)
                    {
                        using (var scopeForEachBid = ScopeFactory.CreateScope())
                        {
                            Response response = await TryUpdateBidPhaseAndNotify(scopeForEachBid, mail, id).ConfigureAwait(false);
                            Console.WriteLine($"UpdatePhase bidId:{id}, success:{response.IsOperationSucceeded}, message:{response.SuccessOrFailureMessage}");
                        }
                    }
                }

                catch (Exception ex) {
                    Console.WriteLine($"Exception was thrown where trying to attampth context.bids in {nameof(UpdateBidsPhaseDaily)}.\nexception details: {ex}");
                }
                // when we exit the using block,
                // the IServiceScope will dispose itself 
                // and dispose all of the services that it resolved.
            }
        }

        public static async Task<Response> TryUpdateBidPhaseAndNotify(IServiceScope scope , IMailService mail , string bidId)
        {
            YotyContext context = scope.ServiceProvider.GetRequiredService<YotyContext>();
            IBidsManager bidsManager = new BidsManager(null, context);
            NotificationsManager notificationsManager = new NotificationsManager(context, mail);

            Response<BidPhase> updatePhaseResponse;
            try
            {
                updatePhaseResponse = await bidsManager.TryUpdatePhase(bidId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            
            Response notificationResponse = null;
            Response modifyDbResponse = null;
            if (!updatePhaseResponse.IsOperationSucceeded)
            {
                Console.WriteLine($"UpdatePhase bidId:{bidId}, no update needed");
                return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = updatePhaseResponse.SuccessOrFailureMessage };
            }
            switch (updatePhaseResponse.DTOObject)
            {
                case BidPhase.Vote:
                    Console.WriteLine($"UpdatePhase bidId:{bidId}, new phase is vote");
                    notificationResponse = await notificationsManager.NotifyBidTimeToVote(bidId).ConfigureAwait(false);
                    modifyDbResponse = await bidsManager.UpdateBidProposalsToRelevant(bidId).ConfigureAwait(false);
                    break;
                case BidPhase.Payment:
                    Console.WriteLine($"UpdatePhase bidId:{bidId}, new phase is payment");
                    notificationResponse = await notificationsManager.NotifyBidTimeToPay(bidId).ConfigureAwait(false);
                    modifyDbResponse = await bidsManager.GetProposalWithMaxVotes(bidId).ConfigureAwait(false);
                    if (modifyDbResponse.IsOperationSucceeded)
                    {
                        // TODO figure better condition
                        notificationResponse = await notificationsManager.NotifyBidChosenSupplier(bidId).ConfigureAwait(false);
                    }
                    break;
                case BidPhase.CancelledSupplierNotFound:
                    Console.WriteLine($"UpdatePhase bidId:{bidId}, new phase is canceled no relevant proposals found");
                    notificationResponse = await notificationsManager.NotifyBidParticipantsSupplierNotFoundCancellation(bidId).ConfigureAwait(false);
                    modifyDbResponse = await bidsManager.UpdateBidProposalsToRelevant(bidId).ConfigureAwait(false);
                    break;
                case BidPhase.CancelledNotEnoughBuyersPayed:
                    Console.WriteLine($"UpdatePhase bidId:{bidId}, new phase is canceled not enough consumers paid");
                    modifyDbResponse = await bidsManager.CancelBid(bidId).ConfigureAwait(false);
                    if (modifyDbResponse.IsOperationSucceeded)
                    {
                        notificationResponse = await notificationsManager.NotifyBidAllMissingPaymentsCancellation(bidId).ConfigureAwait(false);
                    }
                    break;
                case BidPhase.Completed:
                    Console.WriteLine($"UpdatePhase bidId:{bidId}, new phase is completed");
                    modifyDbResponse = await bidsManager.CompleteBid(bidId).ConfigureAwait(false);
                    if (modifyDbResponse.IsOperationSucceeded)
                    {
                        notificationResponse = await notificationsManager.NotifyBidAllCompletion(bidId).ConfigureAwait(false);
                    }
                    break;
            }
            if (notificationResponse != null && !notificationResponse.IsOperationSucceeded)
            {
                return notificationResponse;
            }
            if (modifyDbResponse != null && !modifyDbResponse.IsOperationSucceeded)
            {
                //TODO need to update
                return modifyDbResponse;
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = "TryUpdateBidPhaseAndNotify Success!" };
        }
    }
}
