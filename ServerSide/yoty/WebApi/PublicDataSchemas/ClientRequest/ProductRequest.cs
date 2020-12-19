// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    using Newtonsoft.Json;

    public class ProductRequest
    {
        [JsonProperty("bidId")]
        public string Name { get; set; }

        [JsonProperty("bidId")]
        public Uri Image { get; set; }

        [JsonProperty("bidId")]
        public string Description { get; set; }
    }
}
