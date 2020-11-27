// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Managers
{
    using Microsoft.Extensions.DependencyInjection;
    using YOTY.Service.Managers.Bids;

    public static class ManagersConfigurer
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            return services.AddScoped<IBidsManager, BidManager>();
        }
    }
}