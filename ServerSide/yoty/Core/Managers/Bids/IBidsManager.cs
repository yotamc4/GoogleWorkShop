// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Bids
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YOTY.Service.Data;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public interface IBidsManager
    {
        // create bid
        Task<Response> CreateNewBid(NewBidRequest productBid);

        // get bid details
        Task<Response<BidDTO>> GetBid(string bidId, string userId, string userRole);
        Task<Response<List<BuyerDTO>>> GetBidBuyers(string bidId);
        Task<Response<List<SupplierProposalDTO>>> GetBidSuppliersProposals(string bidId);
        Task<Response<List<ParticipancyDTO>>> GetBidParticipations(string bidId);

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

        Task<Response> MarkPaid(MarkPaidRequest request);

        Task<Response> GetProposalWithMaxVotes(string bidId);

        Task<Response<BidPhase>> TryUpdatePhase(string bidId);

        Task<Response> UpdateBidProposalsToRelevant(string bidId);

        Task<Response> CancelBid(string bidId);

        Task<Response> CompleteBid(string bidId);

        Task<Response<List<OrderDetailsDTO>>> GetPaidCustomersFullOrderDetails(string bidId, string userId);

        Task<Response<List<ParticipancyFullDetailsDTO>>> GetBidParticipationsFullDetails(string bidId);

        Task<Response<SupplierProposalDTO>> GetBidChosenProposal(string bidId);
    }
}
