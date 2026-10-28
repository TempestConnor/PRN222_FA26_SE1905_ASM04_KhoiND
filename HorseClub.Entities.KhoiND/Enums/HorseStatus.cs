namespace HorseClub.Entities.KhoiND.Enums
{
    // Values match CK_Horses_Status; stored in Horse.Status (tinyint).
    public enum HorseStatus : byte
    {
        Active = 0,
        Retired = 1,
        Sold = 2,
        Deceased = 3
    }
}
