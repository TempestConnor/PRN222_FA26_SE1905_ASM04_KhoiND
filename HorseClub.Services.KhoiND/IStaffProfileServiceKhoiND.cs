using HorseClub.Entities.KhoiND.Models;

namespace HorseClub.Services.KhoiND
{
    public interface IStaffProfileServiceKhoiND
    {
        // Queries: read database
        Task<List<StaffProfile>> GetAllAsync();
        Task<StaffProfile?> GetByIdAsync(Guid id);
    }
}
