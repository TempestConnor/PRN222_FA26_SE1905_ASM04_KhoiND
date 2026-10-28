using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND;

namespace HorseClub.Services.KhoiND
{
    public class SupplyItemServiceKhoiND : ISupplyItemServiceKhoiND
    {
        private readonly SupplyItemRepositoryKhoiND _repository;

        public SupplyItemServiceKhoiND()
            => _repository ??= new SupplyItemRepositoryKhoiND();

        public async Task<List<SupplyItem>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all supply items. " + ex.Message, ex);
            }
        }

        public async Task<SupplyItem?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving supply item. " + ex.Message, ex);
            }
        }
    }
}
