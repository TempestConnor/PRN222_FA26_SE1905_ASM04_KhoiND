namespace HorseClub.Entities.KhoiND.Enums
{
    // Values match CK_SupplyCategories_Kind / CK_SupplyItems_Kind;
    // stored in SupplyCategory.Kind and SupplyItem.Kind (tinyint).
    public enum SupplyKind : byte
    {
        Feed = 0,
        Medical = 1,
        Equipment = 2
    }
}
