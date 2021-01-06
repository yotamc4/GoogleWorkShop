// Copyright (c) YOTY Corporation and contributors. All rights reserved.


namespace YOTY.Service.WebApi.Middlewares.Auth
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    using YOTY.Service.Data;

    public class BuyerAuthorizationHandler: AuthorizationHandler<BuyerAuthorizationRequirement>
    {
        private readonly YotyContext dbContext;

        public BuyerAuthorizationHandler(YotyContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BuyerAuthorizationRequirement requirement)
        {

            string buyerId = context.User
                .Claims
                .FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (buyerId == null || await this.dbContext.Buyers.FindAsync(buyerId) == null)
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }
    }
}
