// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Core.Managers
{
    using Microsoft.Extensions.DependencyInjection;
    using UniBuy.Core.Managers.Bids;
    using UniBuy.Core.Managers.Buyers;
    using UniBuy.Core.Managers.Suppliers;

    public static class ManagersConfigurer
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            return services.AddScoped<IBidsManager, BidsManager>()
                    .AddScoped<IBuyersManager, BuyersManager>()
                    .AddScoped<ISuppliersManager, SuppliersManager>();        
        }
    }
}