using PortfolioProject_WebAPI_HotelListing.DataModels;
using System;
using System.Threading.Tasks;

namespace PortfolioProject_WebAPI_HotelListing.IRepository
{
    public interface IUnitOfWork : IDisposable // <- this interface allows for multiple actions to be done in one go
    {                                          //    since repositories only modifie data in a program instance, dont save to db
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotels { get; }
        Task Save();
    }
}
