using HorseClub.Entities.KhoiND.Models;

namespace HorseClub.Services.KhoiND
{
    public interface ISupplyItemServiceKhoiND
    {
        // Queries: read database
        Task<List<SupplyItem>> GetAllAsync();
        Task<SupplyItem?> GetByIdAsync(Guid id);
    }
}
