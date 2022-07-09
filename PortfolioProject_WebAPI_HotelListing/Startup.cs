using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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
            #region Database enableing
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"))
            );
            #endregion

            #region Cross-Origin-Resource-Sharing
            services.AddCors(options => {  // <- Adding Cross-Origin-Resource-Sharing
                options.AddPolicy("CorsPolicy_AllowAll", builder =>
                    builder.AllowAnyOrigin() // <- defines who can access
                           .AllowAnyMethod() // <- defines what methods are allowed tb executed
                           .AllowAnyHeader());
            });
            #endregion

            #region AutoMaper
            services.AddAutoMapper(typeof(MapperInitializer));
            #endregion

            #region UnitOfWork registration
            // Transient means - every time this service is needed new instance will be created for a lifetime of set of requests
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PortfolioProject_WebAPI_HotelListing", Version = "v1" });
            });

            #region Microsoft.AspNetCore.Mvc.NewtonsoftJson
            services.AddControllers().AddNewtonsoftJson(options => 
                                                        options.SerializerSettings.ReferenceLoopHandling
                                                              = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy_AllowAll"); // <- here we simply initiate CorsPolicy built in lines 32:37

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
