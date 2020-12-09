using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public enum BidsSortByOptions
    {
        None,
        ExpirationData,
        PublishDate,
        SupplierProposals,
        DemandedItems,
        Price,
    }

    public enum BidsSortByOrder
    {
        None,
        DESC,
        ACS
    }
}
