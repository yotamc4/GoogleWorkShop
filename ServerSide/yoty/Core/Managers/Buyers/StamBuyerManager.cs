// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Buyers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class StamBuyerManager : IBuyersManager
    {
        public Task<Response<BuyerDTO>> CreateBuyer(NewBuyerRequest newBuyerRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteBuyer(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BidDTO>> GetBidsCreatedByBuyer(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BuyerDTO>> GetBuyer(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DealDTO>> GetBuyerDeals(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BidDTO>> GetBuyerLiveBids(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BidDTO>> GetBuyerOldBids(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BuyerDTO>> GetBuyers(IList<string> buyersIds)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BuyerDTO>> GetBuyers(string productBidId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BuyerDTO>> ModifyBuyerDetails()
        {
            throw new NotImplementedException();
        }
    }
}
