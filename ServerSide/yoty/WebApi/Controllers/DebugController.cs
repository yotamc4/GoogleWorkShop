// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using YOTY.Service.Core.Managers.Bids;
    using YOTY.Service.Core.Managers.Notifications;
    using YOTY.Service.Core.Services.Mail;
    using YOTY.Service.Core.Services.Scheduling;
    using YOTY.Service.WebApi.PublicDataSchemas;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class DebugController : ControllerBase
    {
        private IBidsManager bidsManager;
        private IServiceScopeFactory ScopeFactory;
        private MailSettings MailSettings;
        private MailSecrets MailSecrets;
        private BidsUpdateJobs BidsUpdateJobs;

        public DebugController(IServiceScopeFactory scopeFactory, IBidsManager bidsManager, IOptions<MailSettings> mailSettings, IOptions<MailSecrets> mailSecrets, BidsUpdateJobs bidsUpdateJobs)
        {
            this.bidsManager = bidsManager;
            this.ScopeFactory = scopeFactory;
            MailSettings = mailSettings.Value;
            MailSecrets = mailSecrets.Value;

        }
        [HttpGet]
        [Route("{bidId}/RunBidUpdateJob")]
        public async Task<Response> UpdateBidJob(string bidId)
        {
            IMailService mail = new MailService(MailSettings, MailSecrets);

            using (var scope = ScopeFactory.CreateScope())
            {
                return  await BidsUpdateJobs.TryUpdateBidPhaseAndNotify(scope, mail, bidId).ConfigureAwait(false);
            }
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
