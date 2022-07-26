using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUnitOfWork
    {
        IRepository<Product> ProductRepository { get; }

        IRepository<Product> QueueRepository { get; }

        IRepository<SentProduct> SentRepository { get; }

        IRepository<Characteristic> CharacteristicRepository { get; }

        IRepository<Company> CompanyRepository { get; }
    }
}
