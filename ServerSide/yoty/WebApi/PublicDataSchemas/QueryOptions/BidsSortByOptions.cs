﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public enum BidsSortByOptions
    {
        None,
        ExpirationDate,
        CreationDate,
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
