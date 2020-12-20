// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class VotingRequest
    {
        [JsonProperty("buyerId")]
        public string BuyerId { get; set; }

        [JsonProperty("votedSupplierId")]
        public string VotedSupplierId { get; set; }

        [FromRoute]
        public string BidId { get; set; }
    }
}
