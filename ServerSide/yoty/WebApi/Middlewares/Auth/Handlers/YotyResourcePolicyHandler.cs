// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Middlewares.Auth.Handlers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;

    public abstract class YotyResourcePolicyHandler<TRequirement> : AuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement
    {
        protected static void DetermineAuthorizationResult(AuthorizationHandlerContext context, TRequirement requirement, string attempterUserId, string allowedUserId)
        {
            if (allowedUserId?.Equals(attempterUserId) ?? false)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }

        protected static bool ExtractUserAndResourceIds(AuthorizationHandlerContext context, out string attempterUserId , out string resourceId)
        {
            attempterUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            resourceId = context.Resource as string;
            if (resourceId == null || attempterUserId == null)
            {
                context.Fail();
                return false;
            }
            return true;
        }
    }
}
