namespace HorseClub.Entities.KhoiND.Enums
{
    // Values match CK_StaffProfiles_JobTitle; stored in StaffProfile.JobTitle (tinyint).
    public enum StaffJobTitle : byte
    {
        HeadTrainer = 0,
        Veterinarian = 1,
        Groom = 2,
        ClubManager = 3
    }
}
