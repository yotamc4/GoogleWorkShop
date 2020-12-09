// Copyright (c) YOTY Corporation and contributors. All rights reserved.
namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    using System.Collections.Generic;

    public class ProductDto
    {
        public string Name { get; set; }

        public List<Uri> Images { get; set; }

        public string Description { get; set; }
    }
}
