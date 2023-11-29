namespace TABGCommunityServer
{
    /*internal static class PlayerConcurencyHandler
    {
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>();
        public static byte LastID = 0;

        public static void AddPlayer(Player player)
        {
            Players[player.Id] = player;
            LastID++;
        }

        public static void RemovePlayer(Player player)
        {
            Players.Remove(player.Id);
        }

        public static void UpdatePlayerLocation(int playerId, (float X, float Y, float Z) newLocation)
        {
            if (Players.TryGetValue(playerId, out Player? player))
            {
                player.Location = newLocation;
            }
        }
    }*/
}
