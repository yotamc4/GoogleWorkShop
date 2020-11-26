// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Managers.Bids
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class StamBidManager : IBidsManager
    {
        public Task<Response<BuyerDTO>> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            throw new NotImplementedException();
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
