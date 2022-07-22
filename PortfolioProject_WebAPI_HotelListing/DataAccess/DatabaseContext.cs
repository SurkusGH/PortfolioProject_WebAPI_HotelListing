using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioProject_WebAPI_HotelListing.Configutarions.Entities;
using PortfolioProject_WebAPI_HotelListing.DataModels;

namespace PortfolioProject_WebAPI_HotelListing.DataAccess
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        #region DataSeeding
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CountryConfiguration());
            builder.ApplyConfiguration(new HotelConfiguration());

            builder.ApplyConfiguration(new RoleConfiguration());


        }
        #endregion
    }
}

#region GenerationOfDatabase PackageManagerConsole

// Add-Migration DatabaseCreation
// Update-Database

// Add-Migration SeedingData
// Update-Database

// Add-Migration AddedIdentity
// Update-Database

// Add-Migration AddedDefaultRoles
// Update-Database

#endregion