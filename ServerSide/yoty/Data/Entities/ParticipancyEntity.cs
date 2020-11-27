using System;
using System.ComponentModel.DataAnnotations;

namespace YOTY.Service.Data.Entities
{
    public class ParticipancyEntity
    {
        public string BidId { get; set; }

        public string BuyerId { get; set; }

        public int NumOfUnits { get; set; }

        //-----------------------------
        //Relationships
        [Required]
        public BuyerEntity Buyer { get; set; }

        [Required]
        public ProductBidEntity Bid { get; set; }
    }
}
