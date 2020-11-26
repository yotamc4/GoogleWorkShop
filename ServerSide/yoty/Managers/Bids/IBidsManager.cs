using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Managers.Bids
{
    public interface IBidsManager
    {
        // create bid
        Task<Response<BidDTO>> CreateNewBid(NewBidRequst productBid);

        // get bid details
        Task<Response<BidDTO>> GetBid(string bidId);
        Task<IList<BuyerDTO>> GetBidBuyers(string bidId);
        Task<IList<BuyerDTO>> GetBidSuplliers(string bidId);

        // modify bid
        Task<Response<BuyerDTO>> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest);
        Task<Response> DeleteBuyer(string bidId, string buyerId);

        Task<Response<SupplierProposalDTO>> AddSupplierProposal(SupplierProposalRequest supplierProposal);
        Task<Response> DeleteSupplierProposal(string bidId, string ProposalId);

        Task<Response<BidDTO>> EditBid(EditBidRequest editBidRequest);

        // delete bid 
        Task<Response> DeleteBid(string bidId);

        // get bids
        Task<IList<BidDTO>> GetBids(BidsFilters bidsFilters);

    }
}
