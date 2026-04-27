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

        public async Task UpdateCarPartAsync(string id, CarPart updatedpart)
        {
            await _carPartsCollection.UpdateAsync(id, updatedpart);
        }
    }
}
