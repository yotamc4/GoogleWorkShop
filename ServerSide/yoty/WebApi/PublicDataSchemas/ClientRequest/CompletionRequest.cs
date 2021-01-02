using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace YOTY.Service.WebApi.PublicDataSchemas.ClientRequest
{
    public class CompletionRequest
    {
        [JsonProperty("supplierId")]
        public string SupplierId { get; set; }

        [FromRoute]
        public string BidId { get; set; }
    }
}
