// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using Newtonsoft.Json;

    public class BidBuyerJoinRequest
    {
        [JsonProperty("buyerId")]
        public string BuyerId { get; set; }

        [JsonProperty("bidId")]
        public string BidId { get; set; }

        [JsonProperty("items")]
        public int NumOfUnits { get; set; }

        [JsonProperty("buyerName")]
        public string BuyerName { get; set; }

        [JsonProperty("buyerAddress")]
        public string BuyerAddress { get; set; }

        [JsonProperty("buyerPostalCode")]
        public string BuyerPostalCode { get; set; }

        [JsonProperty("buyerPhoneNumber")]
        public string BuyerPhoneNumber { get; set; }
    }
}
