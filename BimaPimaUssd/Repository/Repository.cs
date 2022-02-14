using BimaPimaUssd.Contracts;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BimaPimaUssd.Repository
{
    public class Repository<T> where T : class
    {
        IMongoDatabase db;
        private readonly string _table;

        public Repository(IStoreDatabaseSettings settings, string table)
        {
            var client = new MongoClient(settings.ConnectionString);
            db = client.GetDatabase(settings.DatabaseName);
            _table = table;
        }
        public void InsertRecord(T record)
        {
            var collection = db.GetCollection<T>(_table);
            collection.InsertOne(record);
        }
        public void InsertMany(List<T> records)
        {
            var collection = db.GetCollection<T>(_table);
            collection.InsertMany(records);
        }
        public List<T> Get() =>
          db.GetCollection<T>(_table).Find(c => true).ToList();

        public T Get(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return db.GetCollection<T>(_table).Find(filter).FirstOrDefault();
        }

        public void Update(string id, T bookIn)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            db.GetCollection<T>(_table).ReplaceOne(filter, bookIn);
        }

        public void Remove(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            db.GetCollection<T>(_table).DeleteOne(filter);
        }
        public T GetByProperty(string property, string value)
        {
            var filter = Builders<T>.Filter.Eq(property, value);
            return db.GetCollection<T>(_table).Find(filter).FirstOrDefault();
        }

    }
}
