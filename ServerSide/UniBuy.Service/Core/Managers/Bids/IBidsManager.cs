// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Core.Managers.Bids
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UniBuy.Service.WebApi.PublicDataSchemas;

    public interface IBidsManager
    {
        // create bid
        Task<Response> CreateNewBid(NewBidRequest productBid);

        // get bid details
        Task<Response<BidDTO>> GetBid(string bidId);
        Task<Response<List<BuyerDTO>>> GetBidBuyers(string bidId);
        Task<Response<List<SupplierProposalDTO>>> GetBidSuplliersProposals(string bidId);

        // modify bid
        Task<Response> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest);
        Task<Response> DeleteBuyer(string bidId, string buyerId);

        Task<Response> AddSupplierProposal(SupplierProposalRequest supplierProposal);
        Task<Response> DeleteSupplierProposal(string bidId, string supplierId);

        Task<Response<BidDTO>> EditBid(EditBidRequest editBidRequest);

        // delete bid 
        Task<Response> DeleteBid(string bidId);

        // get bids
        Task<Response<BidsDTO>> GetBids(BidsQueryOptions bidsFilters);

        Task<Response> VoteForSupplier(VotingRequest votingRequest);
    }
}
