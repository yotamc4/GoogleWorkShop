// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Middlewares.Auth.Handlers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using YOTY.Service.Data;
    using YOTY.Service.WebApi.Middlewares.Auth.Requirements;

    public class ChosenSupplierPolicyHandler: YotyResourcePolicyHandler<ChosenSupplierRequirement>
    {
        private readonly YotyContext dbContext;

        public ChosenSupplierPolicyHandler(YotyContext dbContext):base()
        {
            this.dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ChosenSupplierRequirement requirement)
        {
            if (ExtractUserAndResourceIds(context, out string attempterUserId, out string bidId))
            {
                var bid = await dbContext.Bids.Where(b => b.Id == bidId).Include(b => b.ChosenProposal).FirstOrDefaultAsync().ConfigureAwait(false);
                if (bid == null)
                {
                    context.Succeed(requirement);
                    return;
                }
                string chosenSupplierId = bid.ChosenProposal.SupplierId;
                DetermineAuthorizationResult(context, requirement, attempterUserId, allowedUserId: chosenSupplierId);
            }
        }
    }
}
