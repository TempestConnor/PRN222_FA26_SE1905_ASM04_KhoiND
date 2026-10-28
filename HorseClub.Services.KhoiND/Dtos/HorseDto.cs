using HorseClub.Entities.KhoiND.Models;

namespace HorseClub.Services.KhoiND.Dtos
{
    public record HorseDto(Guid Id, string Name, string? OwnerName, string? Breed);

    public static class HorseDtoMapper
    {
        public static HorseDto ToDto(this Horse e)
            => new(e.Id, e.Name, e.Owner?.FullName, e.Breed);
    }
}
