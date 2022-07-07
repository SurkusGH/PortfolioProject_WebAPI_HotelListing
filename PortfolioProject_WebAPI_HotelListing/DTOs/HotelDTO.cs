using PortfolioProject_WebAPI_HotelListing.DataModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioProject_WebAPI_HotelListing.DTOs
{
    public class CreateHotelDTO
    {
        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "(!) Hotel name is too long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "(!) Adress string is too long")]
        public string Address { get; set; }

        [Required]
        [Range(1,5, ErrorMessage = "(!) Can be double between 0 and 5")]
        public double Rating { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
    }

    public class HotelDTO : CreateHotelDTO
    {
        public int Id { get; set; }
        public CountryDTO Country { get; set; }
    }
}
