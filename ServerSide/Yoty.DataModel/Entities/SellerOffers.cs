using System;
using System.ComponentModel.DataAnnotations;


namespace Yoty.PublicDataSchemas
{
    public class SellerOffer
    {
        [Required]
        public ProductBid Bid { get; set; }

        public string BidId { get; set; }

        public string SellerId { get; set; }

        [Key]
        public string OfferId { get; set; }
        [Required]
        public Seller Seller { get; set; }

        public DateTime PublishedTime { get; set; }

        public int MinimumUnits { get; set; }

        public double OfferedPrice { get; set; }

        public string OfferDescription { get; set; }

    }
}
