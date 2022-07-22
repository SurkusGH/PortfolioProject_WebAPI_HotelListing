using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioProject_WebAPI_HotelListing.DataModels;

namespace PortfolioProject_WebAPI_HotelListing.Configutarions.Entities
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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
        }
    }
}
