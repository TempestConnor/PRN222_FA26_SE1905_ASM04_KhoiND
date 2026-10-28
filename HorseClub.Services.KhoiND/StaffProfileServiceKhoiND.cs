using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND;

namespace HorseClub.Services.KhoiND
{
    public class StaffProfileServiceKhoiND : IStaffProfileServiceKhoiND
    {
        private readonly StaffProfileRepositoryKhoiND _repository;

        public StaffProfileServiceKhoiND()
            => _repository ??= new StaffProfileRepositoryKhoiND();

        public async Task<List<StaffProfile>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all staff profiles. " + ex.Message, ex);
            }
        }

        public async Task<StaffProfile?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving staff profile. " + ex.Message, ex);
            }
        }
    }
}
