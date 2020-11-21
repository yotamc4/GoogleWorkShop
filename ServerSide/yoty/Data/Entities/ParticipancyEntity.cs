using System;
using System.ComponentModel.DataAnnotations;

namespace YOTY.Service.Data.Entities
{
    public class ParticipancyEntity
    {
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
