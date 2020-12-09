// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    using System.Collections.Generic;

    public class NewBidRequst
    {
        public string OwnerId { get; }
        public string Category { get;  }
        public string SubCategory { get; }
        public double MaxPrice { get; }
        public DateTime ExpirationDate { get; set; }
        // TODO: restrict the description size
        ProductRequest Product { get; set; }
    }
}
