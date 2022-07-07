using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PortfolioProject_WebAPI_HotelListing.IRepository
{
    public interface IGenericRepository<T> where T : class // <- syntax that enables T to be passed as a class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null, // <- optional parameter
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, // <- optional parameter
            List<string> included = null
        );
        Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null);
        Task Insert (T entity);
        Task InsertRange(IEnumerable<T> entities);
        Task Delete(int id);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
    }
}
