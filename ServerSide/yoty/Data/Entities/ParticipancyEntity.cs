using System;
using System.ComponentModel.DataAnnotations;
namespace Yoty.Data.Entities
{
    public class ParticipancyEntity
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public ProductBidEntity Bid { get; set; }

        public string BidId { get; set; }

        [Required]
        public BuyerEntity Buyer { get; set; }

        public string BuyerId { get; set; }

        public int NumOfUnits { get; set; }
    }
}
