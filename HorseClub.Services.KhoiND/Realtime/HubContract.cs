namespace HorseClub.Services.KhoiND.Realtime
{
    public static class HubContract
    {
        public const string Path = "/hubs/horseclub";
        // server → client
        public const string ItemChanged  = "ItemChanged";   // (string action, HorseDto item)
        public const string Notification = "Notification";  // (string message)
        // client → server
        public const string Broadcast    = "Broadcast";     // (string message)
    }
}
