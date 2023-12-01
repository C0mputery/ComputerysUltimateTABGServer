namespace TABGCommunityServer.Rooms
{
    public partial class Room
    {
        public Dictionary<byte, Player> Players { get; private set; } = new Dictionary<byte, Player>();
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
    }
}
