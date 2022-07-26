using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private IMongoDatabase _database;
        private readonly string _collectionName;
        private IMongoCollection<T> _collection;

        public Repository(IMongoDatabase database, string collectionName)
        {
            _database = database;
            _collectionName = collectionName;
            _collection = _database.GetCollection<T>(collectionName);
        }

        public void Add(T entity)
        {
            _collection.InsertOneAsync(entity);
        }

        public void Delete(string id) => _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));

        public void DeleteAll() => _collection.DeleteMany(Builders<T>.Filter.Empty);

        public T Get(string id) => IMongoCollectionExtensions.FindSync(_collection, Builders<T>.Filter.Eq("Id", id)).FirstOrDefault();

        public T GetByBarcode(string barcode) => IMongoCollectionExtensions.FindSync(_collection, Builders<T>.Filter.Eq("Barcode", barcode)).FirstOrDefault();

        public T GetByName(string name) => IMongoCollectionExtensions.FindSync(_collection, Builders<T>.Filter.Eq("Name", name)).FirstOrDefault();

        public IEnumerable<T> GetAll() => IMongoCollectionExtensions.FindSync(_collection, Builders<T>.Filter.Empty).ToList();

        public T Update(Expression<Func<T, bool>> filter, T replacement) => _collection.FindOneAndReplace<T>(filter, replacement);
    }
}
