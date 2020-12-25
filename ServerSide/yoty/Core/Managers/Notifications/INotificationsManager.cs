using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Managers.Notifications
{
    public interface INotificationsManager
    {
        Task<Response> Ping();

        Task<Response> NotifyBidAllCompletion(string bidId);

        Task<Response> NotifyBidAllProgressBarCompletion(string bidId);

        Task<Response> NotifyBidSuppliers(string bidId, string body, string subject);

        Task<Response> NotifyBidParticipantsTimeToVote(string bidId);

        Task<Response> NotifyBidParticipantsTimeToPay(string bidId)

        Task<Response> NotifyBidParticipantsSupplierCancellation(string bidId);

        Task<Response> NotifyBidParticipants(string bidId, string body, string subject);

        Task<Response> NotifyBidAll(string bidId, string body, string subject);

        Task<Response> NotifyBidChosenSupplier(string bidId, string body, string subject);
    }
} 