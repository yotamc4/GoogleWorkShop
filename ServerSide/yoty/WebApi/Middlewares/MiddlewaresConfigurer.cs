// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Middlewares
{
    using Microsoft.Extensions.DependencyInjection;
    using YOTY.Service.WebApi.Middlewares.CorrelationId;
    using System;

    public static class MiddlewaresConfigurer
    {

        public static IServiceCollection AddCorrelationIdOptions(this IServiceCollection service, Action<CorrelationIdOptions> optsBuilder = null)
        {
            optsBuilder = optsBuilder ?? (options =>
            {
                options.Header = "X-Correlation-ID";
                options.IncludeInResponse = true;
            });

            service
                .AddOptions<CorrelationIdOptions>()
                .Configure(optsBuilder);
            return service;
        }
    }
}
