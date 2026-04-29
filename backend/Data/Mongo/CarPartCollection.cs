using backend.Data.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace backend.Data.Mongo
{
    public class CarPartCollection : ICarPartCollection
    {
        private readonly IMongoCollection<CarPart> _carPartsCollection;

        public CarPartCollection(
            IMongoClient mongoClient,
            IOptions<DocumentDbSettings> documentDbSettings
        )
        {
            // Initializing
            var mongoDatabase = mongoClient.GetDatabase(documentDbSettings.Value.DatabaseName);
            _carPartsCollection = mongoDatabase.GetCollection<CarPart>(
                documentDbSettings.Value.CollectionName
            );
        }

        public async Task<List<CarPart>> GetAllAsync()
        {
            return await _carPartsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<bool> UpdateAsync(string id, CarPart updaatePart)
        {
            var result = await _carPartsCollection.ReplaceOneAsync(
                part => part.Id == id,
                updaatePart
            );
            return result.ModifiedCount > 0;
        }

        public async Task<CarPart> GetByIdAsync(string id)
        {
            var temp = Builders<CarPart>.Filter.Eq(part => part.Id, id);

            return await _carPartsCollection.Find(temp).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(CarPart newCarPart)
        {
            await _carPartsCollection.InsertOneAsync(newCarPart);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var temp = Builders<CarPart>.Filter.Eq(part => part.Id, id);

            var result = await _carPartsCollection.DeleteOneAsync(temp);

            return result.DeletedCount > 0;
        }
    }
}
