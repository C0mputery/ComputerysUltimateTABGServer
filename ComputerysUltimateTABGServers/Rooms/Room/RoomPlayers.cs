using ComputerysUltimateTABGServers.MiscDataTypes;
using System.Text;
using TABGCommunityServer.MiscDataTypes;

namespace TABGCommunityServer.Rooms
{
    public partial class Room
    {
        public Dictionary<uint, Player> players { get; private set; } = []; // PlayerID, PlayerGroup
        public Dictionary<uint, PlayerGroup> groups { get; private set; } = []; // GroupID, PlayerGroup

        public byte lastPlayerID = 0;

        public void AddPlayer(Player player)
        {
            players[player.m_Peer.ID] = player;
            lastPlayerID++;
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player.m_Peer.ID);
        }

        public void AddPlayerToGroup()
        {

        }
    }
}