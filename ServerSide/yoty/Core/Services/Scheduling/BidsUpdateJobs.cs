using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.Core.Managers.Bids;
using YOTY.Service.Core.Managers.Notifications;
using YOTY.Service.Core.Services.Mail;
using YOTY.Service.Data;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Services.Scheduling
{
    public class BidsUpdateJobs
    {
        public static async Task UpdateBidsPhaseDaily()
        {
            IMailService mail = new MailService(new MailSettings() { DisplayName = "UniBuy", Host = "smtp.gmail.com", Password = "UniBuyIsTheBest", Mail = "unibuy.notifications@gmail.com", Port = 587 });
            YotyContext context = new YotyContext();
            // don't need mapper rn
            IBidsManager bidsManager = new BidsManager(null, context);
            NotificationsManager notificationsManager = new NotificationsManager(context, mail);
            var ids = context.Bids.Select(bid => bid.Id).ToList();
            Response response;
            foreach(var id in ids)
            {
                response = await TryUpdateBidPhaseAndNotify(bidsManager, notificationsManager, id);
                Console.WriteLine($"UpdatePhase bidId:{id}, success:{response.IsOperationSucceeded}, message:{response.SuccessOrFailureMessage}");
            }
        }

        public static async Task<Response> TryUpdateBidPhaseAndNotify(IBidsManager bidsManager, NotificationsManager notificationsManager, string bidId)
        {
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
            Response updateProposalsResponse = null;
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
                    updateProposalsResponse = await bidsManager.UpdateBidProposalsToRelevant(bidId).ConfigureAwait(false);
                    break;
                case BidPhase.Payment:
                    Console.WriteLine($"UpdatePhase bidId:{bidId}, new phase is payment");
                    notificationResponse = await notificationsManager.NotifyBidTimeToPay(bidId).ConfigureAwait(false);
                    updateProposalsResponse = await bidsManager.GetProposalWithMaxVotes(bidId).ConfigureAwait(false);
                    if (updateProposalsResponse.IsOperationSucceeded)
                    {
                        // TODO figure better condition
                        notificationResponse = await notificationsManager.NotifyBidChosenSupplier(bidId).ConfigureAwait(false);
                    }
                    break;
                case BidPhase.CancelledSupplierNotFound:
                    Console.WriteLine($"UpdatePhase bidId:{bidId}, new phase is canceled no relevant proposals found");
                    notificationResponse = await notificationsManager.NotifyBidParticipantsSupplierNotFoundCancellation(bidId).ConfigureAwait(false);
                    break;
            }
            if (notificationResponse != null && !notificationResponse.IsOperationSucceeded)
            {
                return notificationResponse;
            }
            if (updateProposalsResponse != null && !updateProposalsResponse.IsOperationSucceeded)
            {
                //TODO need to update
                return updateProposalsResponse;
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = "TryUpdateBidPhaseAndNotify Success!" };
        }
    }
}
