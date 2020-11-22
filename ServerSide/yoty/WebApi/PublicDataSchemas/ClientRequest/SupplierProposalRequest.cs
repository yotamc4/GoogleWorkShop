using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class SupplierProposalRequest
    {
        string SupplierId { get; }
        DateTime PublishedTime { get; }
        int MinimumUnits { get; }
        double ProposedPrice { get; }
        string Description { get; }
    }
}
