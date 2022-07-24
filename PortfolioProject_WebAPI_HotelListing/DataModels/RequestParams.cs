namespace PortfolioProject_WebAPI_HotelListing.DataModels
{
    public class RequestParams
    {
        public int maxPageSize = 50;
        public int PageNumber { get; set; } = 1; // Default page number will be 1
        private int _pageSize = 10; // Default page size will be 10
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
            

    }
}
