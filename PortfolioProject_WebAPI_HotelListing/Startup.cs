using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PortfolioProject_WebAPI_HotelListing.Configutarions;
using PortfolioProject_WebAPI_HotelListing.DataAccess;
using PortfolioProject_WebAPI_HotelListing.IRepository;
using PortfolioProject_WebAPI_HotelListing.Repository;
using PortfolioProject_WebAPI_HotelListing.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioProject_WebAPI_HotelListing
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
            #region (!) Database enabling
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"))
            );
            #endregion

            services.AddMemoryCache();

            services.ConfigureRateLimiting();
            services.AddHttpContextAccessor();

            services.ConfigureHttpCacheHeaders();

            services.AddResponseCaching();
            services.AddAuthentication();
            services.ConfigureIdentity();

            #region (!) JWT_ConfigurationViaExtension
            services.ConfigureJWT(Configuration);
            #endregion

            #region (!) Cross-Origin-Resource-Sharing setup
            services.AddCors(options => {  // <- Adding Cross-Origin-Resource-Sharing
                options.AddPolicy("CorsPolicy_AllowAll", builder =>
                    builder.AllowAnyOrigin() // <- defines who can access
                           .AllowAnyMethod() // <- defines what methods are allowed tb executed
                           .AllowAnyHeader());
            });
            #endregion

            #region (!) AutoMaper registration
            services.AddAutoMapper(typeof(MapperInitializer));
            #endregion

            #region (!) UnitOfWork registration
            // Transient means - every time this service is needed new instance will be created for a lifetime of set of requests
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion

            #region (!) AuthManager registration
            // Scoped means - method registers the service with a scoped lifetime, the lifetime of a single request. 
            services.AddScoped<IAuthManager, AuthManager>();
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PortfolioProject_WebAPI_HotelListing", Version = "v1" });
            });

            #region Microsoft.AspNetCore.Mvc.NewtonsoftJson
            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                {
                    Duration = 120
                });
            })
                    .AddNewtonsoftJson(options => 
                                                 options.SerializerSettings.ReferenceLoopHandling
                                                         = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.ConfigureVersioning();
            // ^ this part is weird, lecture 19;
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(); // <- these two lines are moved from if statement above, so that they are not conditional
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PortfolioProject_WebAPI_HotelListing v1"));

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy_AllowAll"); // <- here we simply initiate CorsPolicy built in lines 32:37

            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            //app.UseIpRateLimiting(); <-- (!) This Craches app.

            app.UseRouting();

            #region (!) JWTToken enabling middleware
            app.UseAuthentication();
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
