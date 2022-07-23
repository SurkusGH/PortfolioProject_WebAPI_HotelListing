using PortfolioProject_WebAPI_HotelListing.DTOs;
using System.Threading.Tasks;

namespace PortfolioProject_WebAPI_HotelListing.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO userDTO);
        Task<string> CreateToken();
    }
}
