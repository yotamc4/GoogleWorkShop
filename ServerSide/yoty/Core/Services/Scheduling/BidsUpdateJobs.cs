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
        public static async void UpdateBidsPhaseDaily()
        {
            IMailService mail = new MailService(new MailSettings() { DisplayName = "UniBuy", Host = "smtp.gmail.com", Password = "UniBuyIsTheBest", Mail = "unibuy.notifications@gmail.com", Port = 587 });
            YotyContext context = new YotyContext();
            // don't need mapper rn
            IBidsManager bidsManager = new BidsManager(null, context);
            NotificationsManager notificationsManager = new NotificationsManager(context, mail);
            var ids = context.Bids.Select(bid => bid.Id).GetEnumerator();
            Response response;
            ids.Reset();
            while (ids.MoveNext())
            {
                // response = await TryUpdateBidPhaseAndNotify(bidsManager, notificationsManager, ids.Current);
                response = await NotificationsManager.Ping(ids.Current, mail, context);
                Console.WriteLine($"UpdatePhase bidId:{ids.Current}, success:{response.IsOperationSucceeded}, message:{response.SuccessOrFailureMessage}");
            }
            ids.Dispose();
        }

        private static async Task<Response> TryUpdateBidPhaseAndNotify(IBidsManager bidsManager, NotificationsManager notificationsManager, string bidId)
        {
            var updatePhaseResponse = await bidsManager.TryUpdatePhase(bidId).ConfigureAwait(false);

            Response notificationResponse;
            Response updateProposalsResponse;
            if (!updatePhaseResponse.IsOperationSucceeded)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = updatePhaseResponse.SuccessOrFailureMessage };
            }
            switch (updatePhaseResponse.DTOObject)
            {
                case BidPhase.Vote:
                    notificationResponse = await notificationsManager.NotifyBidTimeToVote(bidId).ConfigureAwait(false);
                    updateProposalsResponse = await bidsManager.UpdateBidProposalsToRelevant(bidId).ConfigureAwait(false);
                    break;
                case BidPhase.Payment:
                    notificationResponse = await notificationsManager.NotifyBidTimeToPay(bidId).ConfigureAwait(false);
                    updateProposalsResponse = await bidsManager.UpdateBidProposalsToRelevant(bidId).ConfigureAwait(false);
                    break;
                case BidPhase.CancelledSupplierNotFound:
                    notificationResponse = await notificationsManager.NotifyBidParticipantsSupplierNotFoundCancellation(bidId).ConfigureAwait(false);
                    break;
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = "blabla" };
        }
    }
}
