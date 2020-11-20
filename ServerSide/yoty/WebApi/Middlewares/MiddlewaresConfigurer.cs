using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.Middlewares
{
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
