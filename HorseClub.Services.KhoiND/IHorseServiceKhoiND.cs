using HorseClub.Entities.KhoiND.Models;

namespace HorseClub.Services.KhoiND
{
    public interface IHorseServiceKhoiND
    {
        // Queries: read database
        Task<List<Horse>> GetAllAsync();
        Task<Horse?> GetByIdAsync(Guid id);
        Task<List<Horse>> GetAllAsync(string? name, string? breed, string? ownerName);

        // Mutations: write database
        Task<int> CreateAsync(Horse item);
        Task<int> UpdateAsync(Horse item);
        Task<bool> DeleteAsync(Guid id);
    }
}
