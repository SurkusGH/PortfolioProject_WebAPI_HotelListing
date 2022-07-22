using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioProject_WebAPI_HotelListing.DataModels;

namespace PortfolioProject_WebAPI_HotelListing.Configutarions.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
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
    }
}
