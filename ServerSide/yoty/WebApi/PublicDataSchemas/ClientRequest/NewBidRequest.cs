// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    using Newtonsoft.Json;

    public class NewBidRequest
    {
        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("subCategory")]
        public string SubCategory { get; set; }

        [JsonProperty("maxPrice")]
        public double MaxPrice { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("product")]
        public ProductRequest Product { get; set; }
    }
}
