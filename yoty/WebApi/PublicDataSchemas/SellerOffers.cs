using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class SellerOffer
    {
        string BidId { get; set; }

        string OfferId { get; set; }

        string SellerId { get; set; }

        DateTime PublishedTime { get; set; }

        int MinimumUnits { get; set; }

        double OfferedPrice { get; set; }

        string OfferDescription { get; set; }

    }
}
