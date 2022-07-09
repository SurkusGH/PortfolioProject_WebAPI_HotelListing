using AutoMapper;
using PortfolioProject_WebAPI_HotelListing.DataModels;
using PortfolioProject_WebAPI_HotelListing.DTOs;

namespace PortfolioProject_WebAPI_HotelListing.Configutarions
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Country, CountryDTO>().ReverseMap(); // <- Country model has a direct correlation to DTO and they go both directions
            CreateMap<Country, CreateCountryDTO>().ReverseMap();
            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
        }
    }
}
