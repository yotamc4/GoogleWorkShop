// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Managers.Bids
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YOTY.Service.Data;
    using YOTY.Service.Data.Entities;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class BidManager : IBidsManager
    {
        private static YotyContext _context = new YotyContext();

        public async Task<Response<BuyerDTO>> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            var items = bidBuyerJoinRequest.Items;
            var buyerId = bidBuyerJoinRequest.buyerId;
            var bidId = bidBuyerJoinRequest.productBidId;
            var buyer = await _context.Buyers.FindAsync(buyerId).ConfigureAwait(false);
            var bid = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            var participancy = new ParticipancyEntity() { BidId = bidId, Bid = bid, Buyer = buyer, BuyerId = buyerId, NumOfUnits = items };
            buyer.CurrentParticipancies.Add(participancy);
            bid.CurrentParticipancies.Add(participancy);
            using(var new_context = new YotyContext())
            {
                new_context.Buyers.Update(buyer);
                new_context.Bids.Update(bid);
                await new_context.SaveChangesAsync().ConfigureAwait(false);
            }
            BuyerDTO dto = new BuyerDTO() { Id = buyerId, BuyerAccountDeatails = null, FacebookAccount = null, Name = buyer.Name };
            return new Response<BuyerDTO>();
        }

        public Task<Response<SupplierProposalDTO>> AddSupplierProposal(SupplierProposalRequest supplierProposal)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BidDTO>> CreateNewBid(NewBidRequst productBid)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteBid(string bidId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteBuyer(string bidId, string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteSupplierProposal(string bidId, string ProposalId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BidDTO>> EditBid(EditBidRequest editBidRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BidDTO>> GetBid(string bidId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BuyerDTO>> GetBidBuyers(string bidId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BidDTO>> GetBids()
        {
            throw new NotImplementedException();
        }

        public Task<IList<BidDTO>> GetBids(BidsFilters bidsFilters)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BuyerDTO>> GetBidSuplliers(string bidId)
        {
            throw new NotImplementedException();
        }
    }
}
