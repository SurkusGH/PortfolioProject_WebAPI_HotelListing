using PortfolioProject_WebAPI_HotelListing.DataAccess;
using PortfolioProject_WebAPI_HotelListing.DataModels;
using PortfolioProject_WebAPI_HotelListing.IRepository;
using System;
using System.Threading.Tasks;

namespace PortfolioProject_WebAPI_HotelListing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Country> _countries;
        private IGenericRepository<Hotel> _hotels;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IGenericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_context); // if its empty, return new

        public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_context); // if its empty, return new

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this); // GarbageColector
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
