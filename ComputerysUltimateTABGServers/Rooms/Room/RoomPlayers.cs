using ComputerysUltimateTABGServers.MiscDataTypes;
using ENet;
using System.Text;

namespace ComputerysUltimateTABGServers.Rooms
{
    public partial class Room
    {
        public Dictionary<byte, Player> m_Players { get; private set; } = [];
        public Dictionary<byte, Group> m_Groups { get; private set; } = [];


        public void AddPlayer(Player player)
        {
            m_Players[(byte)player.m_Peer.ID] = player;
            m_Groups[player.m_GroupIndex].m_PlayerIDs.Add((byte)player.m_Peer.ID);
        }

        public void RemovePlayer(Player player)
        {
            m_Players.Remove((byte)player.m_Peer.ID);
        }

        public bool TryToGetPlayer(Peer peer, out Player? player)
        {
            if (m_Players.TryGetValue((byte)peer.ID, out player)) {
                return true;
            }
            return false;
        }

        // I hate how I made this function but fuck it, somebody else can fix it or smth
        public byte FindOrCreateGroup(ulong loginKey, bool autoTeam)
        {
            foreach (Group group in m_Groups.Values.Where(group => group.m_GroupLoginKey == loginKey))
            {
                return group.m_GroupIndex;
            }
            if (autoTeam)
            {
                foreach (Group group in m_Groups.Values.Where(group => group.m_ShouldAutoFill && group.m_PlayerIDs.Count() < group.m_MaxPlayers))
                {
                    return group.m_GroupIndex;
                }
            }
            byte groupIndex = (byte)(m_Groups.Keys.OrderBy(k => k).Last() + 1);
            m_Groups.Add(groupIndex, new Group(autoTeam, loginKey, groupIndex));
            return groupIndex;
        }
    }
}