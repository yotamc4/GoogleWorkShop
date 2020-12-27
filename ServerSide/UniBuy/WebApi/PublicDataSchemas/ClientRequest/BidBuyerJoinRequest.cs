// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.WebApi.PublicDataSchemas
{
    using Newtonsoft.Json;

    public class BidBuyerJoinRequest
    {
        [JsonProperty("buyerId")]
        public string BuyerId { get; set; }

        [JsonProperty("bidId")]
        public string BidId { get; set; }

        [JsonProperty("items")]
        public int Items { get; set; }
    }
}
