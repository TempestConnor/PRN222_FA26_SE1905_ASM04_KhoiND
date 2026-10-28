using HorseClub.Entities.KhoiND.Models;
using HorseClub.Repositories.KhoiND;

namespace HorseClub.Services.KhoiND
{
    public class HorseServiceKhoiND : IHorseServiceKhoiND
    {
        private readonly HorseRepositoryKhoiND _repository;

        public HorseServiceKhoiND()
            => _repository ??= new HorseRepositoryKhoiND();

        public async Task<List<Horse>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all horses. " + ex.Message, ex);
            }
        }

        public async Task<Horse?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving horse. " + ex.Message, ex);
            }
        }

        public async Task<List<Horse>> GetAllAsync(string? name, string? breed, string? ownerName)
        {
            try
            {
                return await _repository.SearchAsync(
                    name?.Trim() ?? string.Empty,
                    breed?.Trim() ?? string.Empty,
                    ownerName?.Trim() ?? string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching horses. " + ex.Message, ex);
            }
        }

        public async Task<int> CreateAsync(Horse item)
        {
            try
            {
                // Id and CreatedAt are left at their defaults so SQL Server fills them
                // (NEWSEQUENTIALID / SYSUTCDATETIME).
                item.IsDeleted = false;
                item.DeletedAt = null;
                return await _repository.CreateAsync(item);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating horse. " + ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(Horse item)
        {
            try
            {
                item.UpdatedAt = DateTime.UtcNow;
                return await _repository.UpdateAsync(item);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating horse. " + ex.Message, ex);
            }
        }

        // Soft delete: the schema keeps rows and flags them with IsDeleted / DeletedAt.
        // The filtered unique indexes (WHERE IsDeleted = 0) free the microchip and
        // passport numbers for reuse once a horse is deleted.
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var item = await _repository.GetByIdAsync(id);
                if (item == null)
                {
                    throw new KeyNotFoundException($"Horse with ID {id} not found.");
                }

                item.Owner = null;
                item.Sire = null;
                item.Dam = null;
                item.IsDeleted = true;
                item.DeletedAt = DateTime.UtcNow;
                return await _repository.UpdateAsync(item) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting horse. " + ex.Message, ex);
            }
        }
    }
}
