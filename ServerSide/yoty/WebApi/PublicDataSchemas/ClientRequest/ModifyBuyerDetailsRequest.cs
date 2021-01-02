using System;
using Newtonsoft.Json;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class ModifyBuyerDetailsRequest
    {

        [JsonProperty("buyerId")]
        public string BuyerId { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("profilePicture")]
        public Uri ProfilePicture { get; set; }
    }
}
