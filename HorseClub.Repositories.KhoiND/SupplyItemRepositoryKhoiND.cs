using Microsoft.EntityFrameworkCore;
using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND.Base;

namespace HorseClub.Repositories.KhoiND
{
    public class SupplyItemRepositoryKhoiND : GenericRepository<SupplyItem>
    {
        public SupplyItemRepositoryKhoiND()
        {
            _context ??= new HorseClubDBContext();
        }

        public SupplyItemRepositoryKhoiND(HorseClubDBContext context)
        {
            _context = context;
        }

        public new async Task<List<SupplyItem>> GetAllAsync()
        {
            return await _context.SupplyItems
                .Include(i => i.Category)
                .Include(i => i.Supplier)
                .Where(i => !i.IsDeleted)
                .OrderBy(i => i.Kind)
                .ThenBy(i => i.Name)
                .ToListAsync();
        }

        public new async Task<SupplyItem?> GetByIdAsync(Guid id)
        {
            return await _context.SupplyItems
                .Include(i => i.Category)
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);
        }
    }
}
