using backend.Data.Entity;

namespace backend.Data.Mongo
{
    public interface ICarPartCollection
    {
        Task<List<CarPart>> GetAllAsync();

        Task<bool> UpdateAsync(string id, CarPart updatedpart);

        Task<CarPart> GetByIdAsync(string id);

        Task CreateAsync(CarPart newCarPart);

        Task<bool> DeleteAsync(string id);
    }
}
