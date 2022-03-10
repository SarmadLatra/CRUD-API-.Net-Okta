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
using Hangfire;
using Hangfire.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD_API.Services.Implementation;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace CRUD_API
{
    public class Startup
    {
        readonly string AllowSpecificOrigins = "_allowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
            });

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
            // Register the Swagger services

            // Register the Swagger services

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Sample API";
                    document.Info.Description = "API For Testing";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Sarmad Saeed",
                        Email = "sarmadsaeed13@gmail.com"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "License",
                        Url = "https://google.com"
                    };
                };
                config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
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

            }

            app.UseCors(AllowSpecificOrigins);
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();
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
