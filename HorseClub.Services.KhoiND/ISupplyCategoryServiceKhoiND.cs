using HorseClub.Entities.KhoiND.Models;

namespace HorseClub.Services.KhoiND
{
    public interface ISupplyCategoryServiceKhoiND
    {
        // Queries: read database
        Task<List<SupplyCategory>> GetAllAsync();
        Task<SupplyCategory?> GetByIdAsync(Guid id);
    }
}
