namespace TABGCommunityServer
{
    internal class PlayerConcurencyHandler
    {
        public Dictionary<int, Player> Players = new Dictionary<int, Player>();
        public byte LastID = 0;

        public void AddPlayer(Player player)
        {
            Players[player.Id] = player;
            LastID++;
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player.Id);
        }

        public void UpdatePlayerLocation(int playerId, (float X, float Y, float Z) newLocation)
        {
            if (Players.TryGetValue(playerId, out Player? player))
            {
                player.Location = newLocation;
            }
        }
    }
}
