using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND;

namespace HorseClub.Services.KhoiND
{
    public class SupplierServiceKhoiND : ISupplierServiceKhoiND
    {
        private readonly SupplierRepositoryKhoiND _repository;

        public SupplierServiceKhoiND()
            => _repository ??= new SupplierRepositoryKhoiND();

        public async Task<List<Supplier>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all suppliers. " + ex.Message, ex);
            }
        }

        public async Task<Supplier?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving supplier. " + ex.Message, ex);
            }
        }
    }
}
