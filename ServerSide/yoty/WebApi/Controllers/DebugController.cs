// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using YOTY.Service.Core.Managers.Bids;
    using YOTY.Service.Core.Managers.Notifications;
    using YOTY.Service.Core.Services.Scheduling;
    using YOTY.Service.WebApi.PublicDataSchemas;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class DebugController : ControllerBase
    {
        private IBidsManager bidsManager;
        private NotificationsManager notificationsManager;

        public DebugController(IBidsManager bidsManager, NotificationsManager notificationsManager)
        {
            this.bidsManager = bidsManager;
            this.notificationsManager = notificationsManager;
        }
        [HttpGet]
        [Route("{bidId}/RunBidUpdateJob")]
        public async Task<Response> UpdateBidJob(string bidId)
        {
            return await BidsUpdateJobs.TryUpdateBidPhaseAndNotify(this.bidsManager, this.notificationsManager, bidId);
        }

        [HttpGet]
        [Route("{bidId}/UpdateProposalsToRelevant")]
        public async Task<Response> UpdateBidProposalsToRelavant(string bidId)
        {
            return await this.bidsManager.UpdateBidProposalsToRelevant(bidId).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{bidId}/UpdateChosenProposal")]
        public async Task<Response> UpdateBidChosenProposal(string bidId)
        {
            return await this.bidsManager.GetProposalWithMaxVotes(bidId).ConfigureAwait(false);
        }
    }
}
