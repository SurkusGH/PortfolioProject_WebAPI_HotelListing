using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PortfolioProject_WebAPI_HotelListing.DataAccess;
using PortfolioProject_WebAPI_HotelListing.DataModels;

namespace PortfolioProject_WebAPI_HotelListing
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
        }
    }
}
