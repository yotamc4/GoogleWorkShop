using System;
using System.ComponentModel.DataAnnotations;
namespace Yoty.Data.Entities
{
    public class SellerOfferEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ProductBidEntity Bid { get; set; }

        public string BidId { get; set; }

        public string SellerId { get; set; }

        [Required]
        public SellerEntity Seller { get; set; }

        public DateTime PublishedTime { get; set; }

        public int MinimumUnits { get; set; }

        public double OfferedPrice { get; set; }

        public string OfferDescription { get; set; }

    }
}
