// Copyright (c) YOTY Corporation and contributors. All rights reserved.


namespace YOTY.Service.WebApi.Middlewares.Auth
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authentication.JwtBearer;

    public static class ServiceAuthConfigurer
    {
        private const string JwtAuthority = "https://dev--1o3sg23.eu.auth0.com/";
        private const string Audience = "https://UniBuyBackend.workshop.com";

        public static IServiceCollection AddYotyAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = JwtAuthority;
                options.Audience = Audience;
                options.RequireHttpsMetadata = false;
            });

            return services; 
        }

        public static IServiceCollection AddYotyAuthorization(this IServiceCollection services)
        {
            return services
                .AddAuthorizationHandler<BuyerAuthorizationHandler>()
                .AddAuthorizationHandler<SupplierAuthorizationHandler>()
                .ConfigureAuthorization(
                    "JwtBearer",
                    CreatePolicyDefinition(PolicyNames.BuyerPolicy, new BuyerAuthorizationRequirement()),
                    CreatePolicyDefinition(PolicyNames.SupplierPolicy, new SupplierAuthorizationRequirement()));
        }

        public static IServiceCollection AddAuthorizationHandler<TAuthorizationHandler>(this IServiceCollection services)
            where TAuthorizationHandler : class, IAuthorizationHandler
        {
            return services.AddScoped<IAuthorizationHandler, TAuthorizationHandler>();
        }

        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, string authenticationScheme, params PolicyDefinition[] policyDefinitions)
        {
            services.AddAuthorization();
            services.Configure<AuthorizationOptions>(authOptions =>
            {
                foreach (PolicyDefinition policyDefinition in policyDefinitions)
                {
                    authOptions.AddPolicy(policyDefinition.PolicyName, policyBuilder =>
                    {
                        policyBuilder.AuthenticationSchemes.Add(authenticationScheme);
                        policyDefinition.ConfigurationAction(policyBuilder);
                    });
                }
            });

            return services;
        }

        private static PolicyDefinition CreatePolicyDefinition(string policyName, IAuthorizationRequirement authorizationRequirement = null)
        {
            return new PolicyDefinition {
                PolicyName = policyName,
                ConfigurationAction = policyBuilder =>
                {
                    policyBuilder.RequireDefaultPolicy();
                    if (authorizationRequirement != null)
                    {
                        policyBuilder.Requirements.Add(authorizationRequirement);
                    }
                },
            };
        }

        private static void RequireDefaultPolicy(this AuthorizationPolicyBuilder policyBuilder)
        {
            policyBuilder.RequireAuthenticatedUser();
            /*
            policyBuilder.RequireClaim(ExtendedClaimTypes.AadTenantId, AllowedTenants);
            foreach (string scope in RequiredScopes)
            {
                policyBuilder.RequireClaimContainsValue(ExtendedClaimTypes.Scope, scope);
            }
            */
        }
    }

}
