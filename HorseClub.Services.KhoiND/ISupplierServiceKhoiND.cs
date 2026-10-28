using HorseClub.Entities.KhoiND.Models;

namespace HorseClub.Services.KhoiND
{
    public interface ISupplierServiceKhoiND
    {
        // Queries: read database
        Task<List<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(Guid id);
    }
}
