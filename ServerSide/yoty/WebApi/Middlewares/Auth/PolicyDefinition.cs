// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Middlewares.Auth
{
    using System;
    using Microsoft.AspNetCore.Authorization;

    public class PolicyDefinition
    {
        public string PolicyName { get; set; }

        public Action<AuthorizationPolicyBuilder> ConfigurationAction { get; set; }
    }
}
