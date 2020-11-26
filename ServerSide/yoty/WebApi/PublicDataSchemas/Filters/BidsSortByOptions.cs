using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public enum BidsSortByOptions
    {
        ExpirationData,
        PublishDate,
        SupplierProposalsCnt,
        DemandedItemsCounter,
        Price,
    }

    public enum BidsSortByOrder
    {
        DESC,
        ACS
    }
}
