using Microsoft.EntityFrameworkCore;
using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND.Base;

namespace HorseClub.Repositories.KhoiND
{
    public class HorseRepositoryKhoiND : GenericRepository<Horse>
    {
        public HorseRepositoryKhoiND()
        {
            _context ??= new HorseClubDBContext();
        }

        public HorseRepositoryKhoiND(HorseClubDBContext context)
        {
            _context = context;
        }

        // Soft-deleted rows (IsDeleted = 1) are never returned.
        public new async Task<List<Horse>> GetAllAsync()
        {
            return await _context.Horses
                .Include(e => e.Owner)
                .Include(e => e.Sire)
                .Include(e => e.Dam)
                .Where(e => !e.IsDeleted)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public new async Task<Horse?> GetByIdAsync(Guid id)
        {
            return await _context.Horses
                .Include(e => e.Owner)
                .Include(e => e.Sire)
                .Include(e => e.Dam)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<List<Horse>> SearchAsync(string name, string breed, string ownerName)
        {
            return await _context.Horses
                .Include(e => e.Owner)
                .Include(e => e.Sire)
                .Include(e => e.Dam)
                .Where(e => !e.IsDeleted
                    && (e.Name.Contains(name) || string.IsNullOrEmpty(name))
                    && (e.Breed.Contains(breed) || string.IsNullOrEmpty(breed))
                    && (e.Owner.FullName.Contains(ownerName) || string.IsNullOrEmpty(ownerName)))
                .OrderBy(e => e.Name)
                .ToListAsync();
        }
    }
}
