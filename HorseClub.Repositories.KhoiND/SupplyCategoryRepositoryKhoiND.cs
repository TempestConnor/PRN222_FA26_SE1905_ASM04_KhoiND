using Microsoft.EntityFrameworkCore;
using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND.Base;

namespace HorseClub.Repositories.KhoiND
{
    public class SupplyCategoryRepositoryKhoiND : GenericRepository<SupplyCategory>
    {
        public SupplyCategoryRepositoryKhoiND()
        {
            _context ??= new HorseClubDBContext();
        }

        public SupplyCategoryRepositoryKhoiND(HorseClubDBContext context)
        {
            _context = context;
        }

        public new async Task<List<SupplyCategory>> GetAllAsync()
        {
            return await _context.SupplyCategories
                .Include(c => c.SupplyItems.Where(i => !i.IsDeleted))
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Kind)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public new async Task<SupplyCategory?> GetByIdAsync(Guid id)
        {
            return await _context.SupplyCategories
                .Include(c => c.SupplyItems.Where(i => !i.IsDeleted))
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }
    }
}
