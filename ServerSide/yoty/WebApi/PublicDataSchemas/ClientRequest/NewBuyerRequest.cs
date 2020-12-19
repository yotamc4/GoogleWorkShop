﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

using Newtonsoft.Json;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class NewBuyerRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        // [JsonProperty("phoneNumber")]
        // public string PhoneNumber { get; set; }

        // [JsonProperty("address")]
        // public string Address { get; set; } // string at the moment
    }
}
