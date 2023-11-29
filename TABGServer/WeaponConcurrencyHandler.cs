namespace TABGCommunityServer
{
    internal static class WeaponConcurrencyHandler
    {
        public static Dictionary<int, Weapon> WeaponDB = new Dictionary<int, Weapon>();
        public static int CurrentID = 0;

        public static void SpawnWeapon(Weapon weapon)
        {
            WeaponDB[weapon.Id] = weapon;
            CurrentID++;
        }

        public static void RemoveWeapon(Weapon weapon)
        {
            WeaponDB.Remove(weapon.Id);
        }
    }
}
