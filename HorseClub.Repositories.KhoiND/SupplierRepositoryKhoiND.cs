using Microsoft.EntityFrameworkCore;
using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND.Base;

namespace HorseClub.Repositories.KhoiND
{
    public class SupplierRepositoryKhoiND : GenericRepository<Supplier>
    {
        public SupplierRepositoryKhoiND()
        {
            _context ??= new HorseClubDBContext();
        }

        public SupplierRepositoryKhoiND(HorseClubDBContext context)
        {
            _context = context;
        }

        public new async Task<List<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .Include(s => s.SupplyItems.Where(i => !i.IsDeleted))
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public new async Task<Supplier?> GetByIdAsync(Guid id)
        {
            return await _context.Suppliers
                .Include(s => s.SupplyItems.Where(i => !i.IsDeleted))
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }
    }
}
