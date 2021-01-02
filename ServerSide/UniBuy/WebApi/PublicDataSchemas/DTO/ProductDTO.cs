// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.WebApi.PublicDataSchemas
{
    using System;
    using System.Collections.Generic;

    public class ProductDTO
    {
        public string Name { get; set; }

        public Uri Image { get; set; }

        public string Description { get; set; }
    }
}
