using CRUD_API.Models;
using CRUD_API.Services.Interfaces;
using CRUD_API.Services.Models;
using CRUD_API.TokenServices;
using Genius.User.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD_API.Services.Implementation;

namespace CRUD_API
{
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://dev-9133002.okta.com/oauth2/default";
                options.Audience = "api://default";
                options.RequireHttpsMetadata = false;
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRUD_API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
            });
            });
            services.Configure<OktaSettings>(Configuration.GetSection("Okta"));
            services.AddMvc();
            services.AddDependency();
            services.AddAutoMapper(typeof(EmplyeeModel).Assembly);
            services.AddControllersWithViews();

            // Hangfire Memory Storage Configuration
            services.AddHangfire(config =>
            config.SetDataCompatibilityLevel(compatibilityLevel: CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseDefaultTypeSerializer()
            .UseMemoryStorage());
            // Hangfire server
            services.AddHangfireServer();
            services.AddSingleton<IPrintJobService, PrintJobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            // Hangfire Simple job Client from DI of .net framework
            IBackgroundJobClient backgroundJobClient,
            // Hangfire Recurring job
            IRecurringJobManager recurringJobManager,
            // Service Provider get serive from DI
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUD_API v1"));
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // For Hangfire Dashboard
            app.UseHangfireDashboard();
            backgroundJobClient.Enqueue(() => Console.WriteLine("Hangfire basic job"));
            recurringJobManager.AddOrUpdate(
                "Hangfire Recurring Job",
                () => serviceProvider.GetService<IPrintJobService>().PrintMessage(),
                Cron.Minutely);
        }
    }
}
