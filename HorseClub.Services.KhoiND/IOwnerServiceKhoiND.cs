using HorseClub.Entities.KhoiND.Models;

namespace HorseClub.Services.KhoiND
{
    public interface IOwnerServiceKhoiND
    {
        // Queries: read database
        Task<List<Owner>> GetAllAsync();
        Task<Owner?> GetByIdAsync(Guid id);
    }
}
