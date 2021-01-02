// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.WebApi.PublicDataSchemas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /*
public class NewBidRequst
{
string OwnerId { get; }
string Category { get;  }
string SubCategory { get; }
double MaxPrice { get; set; }
DateTime ExpirationDate { get; set; }
// TODO: restrict the description size
ProductRequest Product { get; set; }
}
*/
    public class NewBidRequest
    {
        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        //[JsonRequired]
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
