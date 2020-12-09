// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers
{
    using Microsoft.Extensions.DependencyInjection;
    using YOTY.Service.Core.Managers.Bids;

    public static class ManagersConfigurer
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            return services.AddScoped<IBidsManager, StamBidManager>();
        }
    }
}