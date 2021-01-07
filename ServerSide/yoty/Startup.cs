// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace yoty
{
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using YOTY.Service.Core.Managers;
    using YOTY.Service.WebApi.Middlewares;
    using YOTY.Service.WebApi.Middlewares.CorrelationId;
    using YOTY.Service.Data;
    using Microsoft.EntityFrameworkCore;
    using YOTY.Service.Core.Services.Mail;
    using System;
    using Hangfire;
    using Hangfire.SqlServer;
    using YOTY.Service.Core.Services.Scheduling;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using YOTY.Service.WebApi.Middlewares.Auth;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddYotyAuthentication()
                .AddYotyAuthorization();
            string connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = YotyAppData";
            services.AddCors(option => option.AddDefaultPolicy(
                builder => {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    })
            );
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers().AddNewtonsoftJson();
            services.AddCorrelationIdOptions();
            services.AddManagers();
            services.AddDbContext<YotyContext>(options => options.UseSqlServer(connectionString));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            JobStorage.Current = new SqlServerStorage(connectionString);
            // take this line out of comment when DB exists!
            // RecurringJob.AddOrUpdate("UpdateBidsDaily",() => BidsUpdateJobs.UpdateBidsPhaseDaily(), Cron.Daily, TimeZoneInfo.Local);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

        }
    }
}
