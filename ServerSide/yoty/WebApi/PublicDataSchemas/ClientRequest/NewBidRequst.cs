// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
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
    public class NewBidRequst
    {
        [JsonProperty("OwnerId")]
        [Required]
        string OwnerId { get; set; }

        [JsonRequired]
        [JsonProperty("category")]
        string Category { get; set; }

        [JsonProperty("SubCategory")]
        string SubCategory { get; set; }

        [JsonProperty("MaxPrice")]
        double MaxPrice { get; set; }

        [JsonProperty("ExpirationDate")]
        DateTime ExpirationDate { get; set; }

        [JsonProperty("Product")]
        ProductRequest Product { get; set; }
    }
}
