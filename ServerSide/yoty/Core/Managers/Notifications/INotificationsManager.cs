using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Managers.Notifications
{
    public interface INotificationsManager
    {
        Task<Response> NotifyBidAllCompletionAsync(string bidId);

        Task<Response> NotifyBidAllProgressBarCompletionAsync(string bidId);

        Task<Response> NotifyBidTimeToVoteAsync(string bidId);

        Task<Response> NotifyBidTimeToPayAsync(string bidId);

        Task<Response> NotifyBidParticipantsSupplierNotFoundCancellationAsync(string bidId);

        Task<Response> NotifyBidParticipantsSupplierCancellationAsync(string bidId);

        Task<Response> NotifyOnFirstSupplierJoinedToBidAsync(string bidId);

        Task<Response> NotifyOnNewAttractivePropsalForBidAsync(string bidId);

        Task<Response> NotifyOnDemandedUnitFirstReachedProposalThresholdAsync(string bidId);
    }
} 