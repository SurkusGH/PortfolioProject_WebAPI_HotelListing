using System.Collections.Generic;

namespace PortfolioProject_WebAPI_HotelListing.DataModels
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public virtual IList<Hotel> Hotels { get; set; } // <- this does not need migration, only for easyer data manipulation
    }
}
