// Copyright (c) YOTY Corporation and contributors. All rights reserved.



namespace YOTY.Service.WebApi.Middlewares.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using YOTY.Service.Data;

    public class SupplierAuthorizationHandler: AuthorizationHandler<SupplierAuthorizationRequirement>
    {
        private readonly YotyContext dbContext;

        public SupplierAuthorizationHandler(YotyContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SupplierAuthorizationRequirement requirement)
        {
            string supplierId = context.User
                .Claims
                .FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (supplierId == null || await this.dbContext.Suppliers.FindAsync(supplierId) == null)
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }
    }
}
