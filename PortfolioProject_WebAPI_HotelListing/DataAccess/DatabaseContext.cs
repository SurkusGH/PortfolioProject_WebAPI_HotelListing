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
    }
}

#region GenerationOfDatabase PackageManagerConsole

// Add-Migration DatabaseCreation
// Update-Database

#endregion