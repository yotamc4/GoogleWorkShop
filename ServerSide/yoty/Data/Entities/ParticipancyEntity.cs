using System.ComponentModel.DataAnnotations;

namespace YOTY.Service.Data.Entities
{
    public class ParticipancyEntity
    {
        public int NumOfUnits { get; set; }

        public string BuyerName { get; set; }

        public bool HasVoted { get; set; }

        public bool HasPaid{ get; set; }

        //-----------------------------
        //Relationships
        [Required]
        public BuyerEntity Buyer { get; set; }

        [Required]
        public BidEntity Bid { get; set; }

        public string BidId { get; set; }

        public string BuyerId { get; set; }
    }

}
