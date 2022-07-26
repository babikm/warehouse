using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class
    {
        T Get(string id);

        T GetByBarcode(string barcode);

        T GetByName(string name);

        IEnumerable<T> GetAll();

        void Add(T entity);

        void Delete(string id);

        void DeleteAll();

        T Update(Expression<Func<T, bool>> filter, T replacement);
    }
}
