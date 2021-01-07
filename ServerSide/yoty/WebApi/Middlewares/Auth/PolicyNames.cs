// Copyright (c) YOTY Corporation and contributors. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.Middlewares.Auth
{
    public static class PolicyNames
    {
        public const string BuyerPolicy = "BuyerPolicy";

        public const string SupplierPolicy = "SupplierPolicy";

        public const string AdminPolicy = "AdminPolicy";

        public const string BidOwnerPolicy = "BidOwnerPolicy";

        public const string ChosenSupplierPolicy = "ChosenSupplierPolicy";

    }
}
