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
        private readonly IWebHostEnvironment HostingEnvironment;
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var isProd = HostingEnvironment.IsProduction();
            bool isSecretFromKeyVault = Convert.ToBoolean(Configuration["IsServiceSecretFromKeyVault"]);
            bool isLocalDb = Convert.ToBoolean(Configuration["IsLocalDb"]);

            string connectionString = isSecretFromKeyVault ?
                Configuration["UniBuyDBConnectionString"] : 
                isLocalDb ?
                Configuration["LocalDBConnectionString"] : Configuration["ProductionDBSecretConnectionString"];//Configuration.GetConnectionString("localDB");        
            string mailPassword = isSecretFromKeyVault ?  Configuration["UniBuyMailPassword"] : Configuration["EmailPasswordForLocalRun"];

            services
                .AddYotyAuthentication()
                .AddYotyAuthorization();




            services.AddCors(option => option.AddDefaultPolicy(
                builder => {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    })
            );
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers().AddNewtonsoftJson();
            services.AddCorrelationIdOptions();
            services.AddManagers();
            services.AddDbContext<YotyContext>(options => options.UseSqlServer(connectionString));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddOptions<MailSecrets>().Configure(mailSecrets => {
                mailSecrets.Password = mailPassword;
                mailSecrets.ConnectionString = connectionString;
            });
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
            services.AddSingleton<BidsUpdateJobs>();
            // take this line out of comment when DB exists!
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper , BidsUpdateJobs bids)
        {
            RecurringJob.AddOrUpdate("UpdateBidsDaily", () => bids.UpdateBidsPhaseDaily(),Cron.Hourly, TimeZoneInfo.Local);

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
