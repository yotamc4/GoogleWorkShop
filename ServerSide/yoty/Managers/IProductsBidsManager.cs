using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Managers
{
    public interface IProductsBidsManager
    {
        Task<string> CreateNewBid(ProductBid productBid);

        Task AddBuyerToBid(string productBidId, string buyterId, int items);

        Task AddSellerOfferToBid(SellerOffer sellerOffer);

        Task GetBids();

    }
}
