﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    using Newtonsoft.Json;

    public class NewUserRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("userId")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("profilePicture")]
        public Uri ProfilePicture { get; set; }
    }
}
