using System;
using System.ComponentModel.DataAnnotations;
namespace YOTY.Service.Data.Entities
{
    public class SellerOfferEntity
    {
        public string BidId { get; set; }

        public string SellerId { get; set; }

        public DateTime PublishedTime { get; set; }

        public int MinimumUnits { get; set; }

        public double OfferedPrice { get; set; }

        public string OfferDescription { get; set; }

        //-----------------------------
        //Relationships
        [Required]
        public SellerEntity Seller { get; set; }

        [Required]
        public ProductBidEntity Bid { get; set; }
    }
}
