using Microsoft.AspNetCore.Identity;

namespace PortfolioProject_WebAPI_HotelListing.DataModels
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
