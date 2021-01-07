// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Middlewares.Auth.Handlers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using YOTY.Service.Data;
    using YOTY.Service.WebApi.Middlewares.Auth.Requirements;

    public class BidOwnerPolicyHandler: YotyResourcePolicyHandler<BidOwnerRequirement>
    {
        private readonly YotyContext dbContext;

        public BidOwnerPolicyHandler(YotyContext dbContext) : base()
        {
            this.dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BidOwnerRequirement requirement)
        {

            if (ExtractUserAndResourceIds(context,out string attempterUserId, out string bidId))
            {
                var bid = await dbContext.Bids.FindAsync(bidId).ConfigureAwait(false);
                if(bid ==null)
                {
                    context.Succeed(requirement);
                    return;
                }
                string bidOwnerId = bid.OwnerId;
                DetermineAuthorizationResult(context, requirement, attempterUserId, allowedUserId: bidOwnerId);
            }
        }
    }
}
