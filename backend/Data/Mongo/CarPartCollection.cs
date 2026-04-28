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
            // Initializing
            var mongoClient = new MongoClient(documentDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(documentDbSettings.Value.DatabaseName);
            _carPartsCollection = mongoDatabase.GetCollection<CarPart>(
                documentDbSettings.Value.CollectionName
            );
        }

        public async Task<List<CarPart>> GetAllAsync()
        {
            return await _carPartsCollection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(string id, CarPart updaatePart)
        {
            await _carPartsCollection.ReplaceOneAsync(part => part.Id == id, updaatePart);
        }

        public async Task<CarPart> GetByIdAsync(string id)
        {
            var tempId = Builders<CarPart>.Filter.Eq(part => part.Id, id);

            return await _carPartsCollection.Find(tempId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(CarPart newCarPart)
        {
            await _carPartsCollection.InsertOneAsync(newCarPart);
        }
    }
}
