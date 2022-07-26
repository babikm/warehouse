using DAL;
using MongoDB.Driver;
using System.Configuration;
using System.Xml.Serialization;

namespace Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private IMongoDatabase _database;
        private IRepository<Product> _queueRepository;
        private IRepository<SentProduct> _sentRepository;
        private IRepository<Characteristic> _characteristicRepository;
        private IRepository<Company> _companyRepository;

        public UnitOfWork()
        {

            var path = System.IO.File.ReadAllText(@".\databaseSettings.txt");
            var client = new MongoClient(path);
            var dbName = "PlastMMDb";
            _database = client.GetDatabase(dbName);
        }

        private IRepository<Product> _productRepository;
        public IRepository<Product> ProductRepository
            => _productRepository ?? (_productRepository = new Repository<Product>(_database, "Products"));

        public IRepository<Product> QueueRepository => _queueRepository ??= new Repository<Product>(_database, "Queue");

        public IRepository<SentProduct> SentRepository => _sentRepository ??= new Repository<SentProduct>(_database, "Sent");

        public IRepository<Characteristic> CharacteristicRepository => _characteristicRepository ??= new Repository<Characteristic>(_database, "Characteristic");

        public IRepository<Company> CompanyRepository => _companyRepository ??= new Repository<Company>(_database, "Company");
    }
}
