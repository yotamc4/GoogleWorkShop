// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Core.Managers
{
    using Microsoft.Extensions.DependencyInjection;
    using UniBuy.Service.Core.Managers.Bids;
    using UniBuy.Service.Core.Managers.Buyers;
    using UniBuy.Service.Core.Managers.Suppliers;

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