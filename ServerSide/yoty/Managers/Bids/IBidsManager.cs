using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Managers.Bids
{
    public interface IBidsManager
    {
        Task<string> CreateNewBid(Bid productBid);

        Task AddBuyerToBid(string productBidId, string buyerId, BidBuyerJoinRequest bidBuyerJoinRequest);

        Task AddSellerOfferToBid(SellerOffer sellerOffer);

        Task GetBids();

        Task<IList<Buyer>> GetBuyers(string productBidId);

    }
}
