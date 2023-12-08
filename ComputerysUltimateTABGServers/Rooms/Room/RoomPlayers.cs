using ComputerysUltimateTABGServers.MiscDataTypes;
using ENet;
using System.Text;

namespace ComputerysUltimateTABGServers.Rooms
{
    public partial class Room
    {
        public Dictionary<byte, Player> players { get; private set; } = [];
        public Dictionary<byte, Group> groups { get; private set; } = [];


        public void AddPlayer(Player player)
        {
            players[(byte)player.m_Peer.ID] = player;
        }

        public void RemovePlayer(Player player)
        {
            players.Remove((byte)player.m_Peer.ID);
        }

        public bool TryToGetPlayer(Peer peer, out Player? player)
        {
            if (players.TryGetValue((byte)peer.ID, out player)) {
                return true;
            }
            return false;
        }

        public byte JoinOrCreateGroup(Peer peer, ulong loginKey, bool autoTeam)
        {
            List<byte> sortedKeys = groups.Keys.OrderBy(k => k).ToList();
            byte groupIndex = (byte)(sortedKeys.Last() + 1);
            groups.Add(groupIndex, new Group());
            return groupIndex;
        }
    }
}