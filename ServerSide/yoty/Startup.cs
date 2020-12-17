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
    using Newtonsoft.Json;
    using YOTY.Service.Data;
    using Microsoft.EntityFrameworkCore;

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
            services.AddDbContext<YotyContext>(options => options.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = YotyAppData"));
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
