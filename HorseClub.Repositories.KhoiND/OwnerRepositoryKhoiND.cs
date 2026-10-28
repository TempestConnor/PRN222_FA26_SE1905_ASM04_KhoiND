using Microsoft.EntityFrameworkCore;
using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND.Base;

namespace HorseClub.Repositories.KhoiND
{
    public class OwnerRepositoryKhoiND : GenericRepository<Owner>
    {
        public OwnerRepositoryKhoiND()
        {
            _context ??= new HorseClubDBContext();
        }

        public OwnerRepositoryKhoiND(HorseClubDBContext context)
        {
            _context = context;
        }

        public new async Task<List<Owner>> GetAllAsync()
        {
            return await _context.Owners
                .Include(p => p.Horses)
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.FullName)
                .ToListAsync();
        }

        public new async Task<Owner?> GetByIdAsync(Guid id)
        {
            return await _context.Owners
                .Include(p => p.Horses)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }
    }
}
