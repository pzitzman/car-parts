using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Data.Entity;
using backend.Data.Mongo;

namespace backend.Model
{
    public class CarPartModel
    {
        private readonly ICarPartCollection _carPartsCollection;

        public CarPartModel(ICarPartCollection carPartCollection)
        {
            _carPartsCollection = carPartCollection;
        }

        public async Task<List<CarPart>> GetAllCarPartsAsync()
        {
            return await _carPartsCollection.GetAllAsync();
        }

        public async Task UpdateCarPartAsync(string id, CarPart updatedPart)
        {
            var tempPart = await _carPartsCollection.GetByIdAsync(id);
            if (tempPart == null)
            {
                throw new ArgumentException("Part not found");
            }

            tempPart.Name = updatedPart.Name.Trim();
            tempPart.PartNumber = updatedPart.PartNumber.Trim();
            tempPart.Description = updatedPart.Description.Trim();

            tempPart.UpdatedAt = DateTime.UtcNow;

            await _carPartsCollection.UpdateAsync(id, tempPart);
        }

        public async Task CreateCarPartAsync(CarPart newCarPart)
        {
            if (newCarPart == null)
            {
                throw new ArgumentNullException(nameof(newCarPart));
            }
            if (string.IsNullOrWhiteSpace(newCarPart.Name))
            {
                throw new ArgumentException("Must have a name");
            }

            var now = DateTime.UtcNow;
            newCarPart.CreatedAt = now;
            newCarPart.UpdatedAt = now;

            await _carPartsCollection.CreateAsync(newCarPart);
        }
    }
}
