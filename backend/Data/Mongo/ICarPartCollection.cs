using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Data.Entity;

namespace backend.Data.Mongo
{
    public interface ICarPartCollection
    {
        Task<List<CarPart>> GetAllAsync();

        Task UpdateAsync(string id, CarPart updatedpart);

        Task<CarPart> GetByIdAsync(string id);

        Task CreateAsync(CarPart newCarPart);
    }
}
