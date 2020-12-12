using System;
using System.ComponentModel.DataAnnotations;
namespace YOTY.Service.Data.Entities
{
    public class SupplierProposalEntity
    {
        public DateTime PublishedTime { get; set; }

        public int MinimumUnits { get; set; }

        public double OfferedPrice { get; set; }

        public string OfferDescription { get; set; }

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
