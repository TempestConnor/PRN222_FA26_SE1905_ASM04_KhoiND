using Microsoft.EntityFrameworkCore;
using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND.Base;

namespace HorseClub.Repositories.KhoiND
{
    public class StaffProfileRepositoryKhoiND : GenericRepository<StaffProfile>
    {
        public StaffProfileRepositoryKhoiND()
        {
            _context ??= new HorseClubDBContext();
        }

        public StaffProfileRepositoryKhoiND(HorseClubDBContext context)
        {
            _context = context;
        }

        public new async Task<List<StaffProfile>> GetAllAsync()
        {
            return await _context.StaffProfiles
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.FullName)
                .ToListAsync();
        }

        public new async Task<StaffProfile?> GetByIdAsync(Guid id)
        {
            return await _context.StaffProfiles
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }
    }
}
