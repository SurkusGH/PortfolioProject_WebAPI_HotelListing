using Microsoft.EntityFrameworkCore;
using PortfolioProject_WebAPI_HotelListing.DataModels;

namespace PortfolioProject_WebAPI_HotelListing.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        #region DataSeeding
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    ShortName = "LT"
                },
                new Country
                {
                    Id = 2,
                    Name = "Latvia",
                    ShortName = "LV"
                },
                new Country
                {
                    Id = 3,
                    Name = "Estonia",
                    ShortName = "EE"
                }
                );
            builder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sostinė",
                    Address = "Sostinės g. 100",
                    CountryId = 1,
                    Rating = 4.0
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Express Hotel",
                    Address = "Rigos g. 20",
                    CountryId = 2,
                    Rating = 3.5
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Riga",
                    Address = "Talino g. 5",
                    CountryId = 3,
                    Rating = 4.5
                }
                );
        }
        #endregion
    }
}

#region GenerationOfDatabase PackageManagerConsole

// Add-Migration DatabaseCreation
// Update-Database

// Add-Migration SeedingData
// Update-Database

#endregion