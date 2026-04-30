using backend.Data.Entity;
using backend.Data.Mongo;
using backend.DTOs;

namespace backend.Model
{
    public class CarPartModel
    {
        private readonly ICarPartCollection _carPartsCollection;

        public CarPartModel(ICarPartCollection carPartCollection)
        {
            _carPartsCollection = carPartCollection;
        }

        public async Task<List<CarPartGetDto>> GetAllCarPartsAsync()
        {
            var parts = await _carPartsCollection.GetAllAsync();

            return parts
                .Select(part => new CarPartGetDto
                {
                    Id = part.Id,
                    Name = part.Name,
                    PartNumber = part.PartNumber,
                    Description = part.Description,
                    CreatedAt = part.CreatedAt,
                    UpdatedAt = part.UpdatedAt,
                })
                .ToList();
        }

        public async Task<CarPartGetDto> GetCarPartByIdAsync(string id)
        {
            var tempPart = await _carPartsCollection.GetByIdAsync(id);

            if (tempPart == null)
            {
                throw new KeyNotFoundException($"Car Part not found with{id}");
            }

            return new CarPartGetDto
            {
                Id = tempPart.Id,
                Name = tempPart.Name,
                PartNumber = tempPart.PartNumber,
                Description = tempPart.Description,
                CreatedAt = tempPart.CreatedAt,
                UpdatedAt = tempPart.UpdatedAt,
            };
        }

        public async Task UpdateCarPartAsync(string id, CarPartDto updatedPart)
        {
            if (updatedPart == null)
            {
                throw new ArgumentNullException("Reqeust body missing");
            }
            var tempPart = await _carPartsCollection.GetByIdAsync(id);
            if (tempPart == null)
            {
                throw new KeyNotFoundException($"Car Part not found with{id}");
            }

            tempPart.Name = updatedPart.Name;
            tempPart.PartNumber = updatedPart.PartNumber;
            tempPart.Description = updatedPart.Description;
            tempPart.UpdatedAt = DateTime.UtcNow;

            bool wasUpdated = await _carPartsCollection.UpdateAsync(id, tempPart);

            if (!wasUpdated)
            {
                throw new KeyNotFoundException($"Car Part not updated with{id}");
            }
        }

        public async Task<CarPartGetDto> CreateCarPartAsync(CarPartDto newCarPart)
        {
            if (newCarPart == null)
            {
                throw new ArgumentNullException("Reqeust body missing");
            }
            var now = DateTime.UtcNow;
            var tempPart = new CarPart
            {
                Name = newCarPart.Name,
                PartNumber = newCarPart.PartNumber,
                Description = newCarPart.Description,
                CreatedAt = now,
                UpdatedAt = now,
            };

            await _carPartsCollection.CreateAsync(tempPart);

            return new CarPartGetDto
            {
                Id = tempPart.Id,
                Name = tempPart.Name,
                PartNumber = tempPart.PartNumber,
                Description = tempPart.Description,
                CreatedAt = tempPart.CreatedAt,
                UpdatedAt = tempPart.UpdatedAt,
            };
        }

        public async Task DeleteCarPartAsync(string id)
        {
            bool wasDeleted = await _carPartsCollection.DeleteAsync(id);

            if (!wasDeleted)
            {
                throw new KeyNotFoundException($"Car Part not found with{id}");
            }
        }
    }
}
