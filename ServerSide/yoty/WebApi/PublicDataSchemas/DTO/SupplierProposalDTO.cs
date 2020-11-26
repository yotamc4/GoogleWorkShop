using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class SupplierProposalDTO: BaseDTO
    {
        string BidId { get; set; }

        string SupplierName { get; set; }

        string ProposalId { get; set; }

        string SupplierId { get; set; }

        DateTime PublishedTime { get; set; }

        int MinimumUnits { get; set; }

        double ProposedPrice { get; set; }

        string Description { get; set; }

    }
}
