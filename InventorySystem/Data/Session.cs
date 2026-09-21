namespace InventorySystem.Data
{
    public static class Session
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string FullName { get; set; }
        public static string RoleName { get; set; }

        public static bool IsAdmin => RoleName == "Administrator";
        public static bool IsManager => RoleName == "Administrator" || RoleName == "Manager";

        public static void Clear()
        {
            UserID = 0; Username = FullName = RoleName = null;
        }
    }
}