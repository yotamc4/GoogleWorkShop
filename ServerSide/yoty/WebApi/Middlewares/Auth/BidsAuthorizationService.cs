// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Middlewares.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using YOTY.Service.Data;

    public class BidsAuthorizationService : IAuthorizationService
    {
        private readonly YotyContext dbContext;

        public BidsAuthorizationService(YotyContext dbContext)
        {
            this.dbContext = dbContext;

        }
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            string attempterUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string bidId = resource as string;
            if (bidId == null || attempterUserId == null)
            {
                return AuthorizationResult.Failed();
            }

            switch (policyName)
            {
                case PolicyNames.BidOwnerPolicy:
                    return await this.HandleBidOwnerPolicyAsync(bidId, attempterUserId);
                case PolicyNames.ChosenSupplierPolicy:
                    return await this.HandleChosenSupplierPolicyAsync(bidId, attempterUserId);
            }

            return AuthorizationResult.Success();
        }

        private async Task<AuthorizationResult> HandleChosenSupplierPolicyAsync(string bidId, string attempterUserId)
        {
            
            var bid = await dbContext.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentParticipancies).ThenInclude(p => p.Buyer).Include(b => b.ChosenProposal).FirstOrDefaultAsync().ConfigureAwait(false);
            string chosenSupplierId = bid.ChosenProposal.SupplierId;
            return DetermineAuthorizationResult(attempterUserId, allowedUserId: chosenSupplierId);
        }

        private async Task<AuthorizationResult> HandleBidOwnerPolicyAsync(string bidId, string attempterUserId)
        {
            var bid = await dbContext.Bids.FindAsync(bidId).ConfigureAwait(false);
            string bidOwnerId = bid.OwnerId;
            return DetermineAuthorizationResult(attempterUserId, allowedUserId: bidOwnerId);
        }

        private static  AuthorizationResult DetermineAuthorizationResult(string attempterUserId, string allowedUserId)
        {
            if (allowedUserId?.Equals(attempterUserId) ?? false)
            {
                return AuthorizationResult.Success();
            }

            return AuthorizationResult.Failed();
        }
    }
}
