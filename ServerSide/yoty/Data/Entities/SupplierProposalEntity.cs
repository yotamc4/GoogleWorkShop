using System;
using System.ComponentModel.DataAnnotations;
namespace YOTY.Service.Data.Entities
{
    public class SupplierProposalEntity
    {
        public DateTime PublishedTime { get; set; }

        public int MinimumUnits { get; set; }

        public double ProposedPrice { get; set; }

        public string Description { get; set; }

        public string SupplierName { get; set; }

        public int VotesCounter { get; set; }

        //-----------------------------
        //Relationships
        [Required]
        public SupplierEntity Supplier { get; set; }

        [Required]
        public BidEntity Bid { get; set; }

        public string BidId { get; set; }

        public string SupplierId { get; set; }

    }
}
