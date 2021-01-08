using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Managers.Notifications
{
    public interface INotificationsManager
    {
        Task<Response> NotifyBidAllCompletion(string bidId);

        Task<Response> NotifyBidAllProgressBarCompletion(string bidId);

        Task<Response> NotifyBidTimeToVote(string bidId);

        Task<Response> NotifyBidTimeToPay(string bidId);

        Task<Response> NotifyBidParticipantsSupplierNotFoundCancellation(string bidId);

        Task<Response> NotifyBidParticipantsNotPaidCancellation(string bidId);
    }
} 