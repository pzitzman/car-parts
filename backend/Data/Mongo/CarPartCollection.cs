using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Data.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace backend.Data.Mongo
{
    public class CarPartCollection : ICarPartCollection
    {
        private readonly IMongoCollection<CarPart> _carPartsCollection;

        public CarPartCollection(IOptions<DocumentDbSettings> documentDbSettings)
        {
            // Initialize MongoClient, get DB and bind CarPart
            var mongoClient = new MongoClient(documentDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(documentDbSettings.Value.DatabaseName);
            _carPartsCollection = mongoDatabase.GetCollection<CarPart>(
                documentDbSettings.Value.CollectionName
            );
        }
    }
}
