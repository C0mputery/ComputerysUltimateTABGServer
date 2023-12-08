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

        // I hate how I made this function but fuck it, somebody else can fix it or smth
        public byte FindOrCreateGroup(ulong loginKey, bool autoTeam)
        {
            foreach (Group group in groups.Values.Where(group => group.m_GroupLoginKey == loginKey))
            {
                return group.m_GroupIndex;
            }
            if (autoTeam)
            {
                foreach (Group group in groups.Values.Where(group => group.m_ShouldAutoFill && group.m_Players.Count() < group.m_MaxPlayers))
                {
                    return group.m_GroupIndex;
                }
            }
            byte groupIndex = (byte)(groups.Keys.OrderBy(k => k).Last() + 1);
            groups.Add(groupIndex, new Group(autoTeam, loginKey, groupIndex));
            return groupIndex;
        }
    }
}