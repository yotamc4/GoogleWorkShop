// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    using Newtonsoft.Json;

    public class EditBidRequest
    {
        [JsonProperty("bidId")]
        public string BidId { get; set; }

        [JsonProperty("newName")]
        public string NewName { get; set; }

        [JsonProperty("newProductImage")]
        public Uri NewProductImage { get; set; }

        [JsonProperty("newDescription")]
        public string NewDescription { get; set; }

        [JsonProperty("newCategory")]
        public string NewCategory { get; set; }

        [JsonProperty("newSubCategory")]
        public string NewSubCategory { get; set; }

        //note: adding things here validate EditBid in manager is updated
    }
}
