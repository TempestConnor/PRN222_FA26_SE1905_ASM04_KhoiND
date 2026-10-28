using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND;

namespace HorseClub.Services.KhoiND
{
    public class SupplyCategoryServiceKhoiND : ISupplyCategoryServiceKhoiND
    {
        private readonly SupplyCategoryRepositoryKhoiND _repository;

        public SupplyCategoryServiceKhoiND()
            => _repository ??= new SupplyCategoryRepositoryKhoiND();

        public async Task<List<SupplyCategory>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all supply categories. " + ex.Message, ex);
            }
        }

        public async Task<SupplyCategory?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving supply category. " + ex.Message, ex);
            }
        }
    }
}
