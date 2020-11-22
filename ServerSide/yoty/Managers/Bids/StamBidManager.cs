// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Managers.Bids
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class StamBidManager : IBidsManager
    {
        public Task<Response> AddBuyerToBid(BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SupplierProposalDTO>> AddSupplierProposalDTOToBid(SupplierProposalRequest sellerOffer)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BidDTO>> CreateNewBid(NewBidRequst productBid)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BidDTO>> GetBid(string bidId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BidDTO>> GetBids()
        {
            throw new NotImplementedException();
        }

        public Task<IList<BuyerDTO>> GetBuyers(string productBidId)
        {
            throw new NotImplementedException();
        }
    }
}
