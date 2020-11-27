using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class SupplierProposalRequest
    {
        public string BidId { get; }
        public string SupplierId { get; }
        public DateTime PublishedTime { get; }
        public int MinimumUnits { get; }
        public double ProposedPrice { get; }
        public string Description { get; }
    }
}
