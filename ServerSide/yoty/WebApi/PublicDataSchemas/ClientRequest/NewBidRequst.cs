// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    using System.Collections.Generic;

    public class NewBidRequst
    {
        string OwnerId { get; }
        string Category { get;  }
        string SubCategory { get; }
        double MaxPrice { get; }
        DateTime ExpirationDate { get; set; }
        // TODO: restrict the description size
        ProductRequest Product { get; set; }
    }
}
