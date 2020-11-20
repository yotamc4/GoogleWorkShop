using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Managers.Bids
{
    public class BidManager : IBidsManager
    {
        public Task AddBuyerToBid(string productBidId, string buyerId, BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            throw new NotImplementedException();
        }

        public Task AddSellerOfferToBid(SellerOffer sellerOffer)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateNewBid(Bid productBid)
        {
            throw new NotImplementedException();
        }

        public Task GetBids()
        {
            throw new NotImplementedException();
        }

        public Task<IList<Buyer>> GetBuyers(string productBidId)
        {
            throw new NotImplementedException();
        }
    }
}
