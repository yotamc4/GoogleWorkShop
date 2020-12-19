// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    using Newtonsoft.Json;

    public class SupplierProposalRequest
    {
        [JsonProperty("bidId")]
        public string BidId { get; set; }

        [JsonProperty("supplierId")]
        public string SupplierId { get; set; }
        
        [JsonProperty("supplierName")]
        public string SupplierName { get; set; }

        [JsonProperty("publishedTime")]
        public DateTime PublishedTime { get; set; }

        [JsonProperty("minimumUnits")]
        public int MinimumUnits { get; set; }

        [JsonProperty("proposedPrice")]
        public double ProposedPrice { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}