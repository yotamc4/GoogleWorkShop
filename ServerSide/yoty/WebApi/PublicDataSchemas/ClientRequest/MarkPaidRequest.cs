using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace YOTY.Service.WebApi.PublicDataSchemas.ClientRequest
{
    public class MarkPaidRequest
    {
        [JsonProperty("buyerId")]
        public string BuyerId { get; set; }

        // The user who marks this
        [JsonProperty("markingUserId")]
        public string MarkingUserId { get; set; }

        [JsonProperty("hasPaid")]
        public bool HasPaid { get; set; }

        [FromRoute]
        public string BidId { get; set; }
    }
}
