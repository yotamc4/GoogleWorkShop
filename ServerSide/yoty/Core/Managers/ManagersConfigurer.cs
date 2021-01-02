// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers
{
    using Microsoft.Extensions.DependencyInjection;
    using YOTY.Service.Core.Managers.Bids;
    using YOTY.Service.Core.Managers.Buyers;
    using YOTY.Service.Core.Managers.Suppliers;
    using YOTY.Service.Core.Managers.Notifications;

    public static class ManagersConfigurer
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            return services.AddScoped<IBidsManager, BidsManager>()
                    .AddScoped<IBuyersManager, BuyersManager>()
                    .AddScoped<ISuppliersManager, SuppliersManager>()
                    .AddScoped<INotificationsManager, NotificationsManager>();
        }
    }
}