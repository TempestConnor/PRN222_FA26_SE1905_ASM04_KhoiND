using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND;

namespace HorseClub.Services.KhoiND
{
    public class OwnerServiceKhoiND : IOwnerServiceKhoiND
    {
        private readonly OwnerRepositoryKhoiND _repository;

        public OwnerServiceKhoiND()
            => _repository ??= new OwnerRepositoryKhoiND();

        public async Task<List<Owner>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all owners. " + ex.Message, ex);
            }
        }

        public async Task<Owner?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving owner. " + ex.Message, ex);
            }
        }
    }
}
