using System;
using Newtonsoft.Json;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class ModifySupplierDetailsRequest
    {
        [JsonProperty("supplierId")]
        public string SupplierId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("profilePicture")]
        public Uri ProfilePicture { get; set; }

        [JsonProperty("paymentLink")]
        public Uri PaymentLink { get; set; }
    }
}
