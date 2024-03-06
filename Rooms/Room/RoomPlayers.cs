using ComputerysUltimateTABGServer.DataTypes.Player;
using ENet;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room
    {
        // This is a concurrent dictionary because it is read by multiple threads at once, if you think this is unnecessary, you can change it to a normal dictionary.
        public ConcurrentDictionary<byte, Player> m_Players { get; private set; } = [];
        public Dictionary<byte, Group> m_Groups { get; private set; } = [];

        public int m_PlayerLives;
        public ushort m_KillsToWin;

        public void AddPlayer(Player player)
        {
            m_Players[player.m_PlayerID] = player;
            m_Groups[player.m_GroupIndex].m_PlayerIDs.Add(player.m_PlayerID);
        }
        public void RemovePlayer(Player player)
        {
            m_Players.TryRemove(player.m_PlayerID, out _);
        }

        public bool TryToGetPlayer(Peer peer, [MaybeNullWhen(false)] out Player player)
        {
            player = m_Players.Values.FirstOrDefault(p => p.m_Peer.Equals(peer));
            return player != null;
        }
        public bool TryToGetPlayer(byte playerID, [MaybeNullWhen(false)] out Player player)
        {
            m_Players.TryGetValue(playerID, out player);
            return player != null;
        }
        public bool TryToGetPlayer(Peer peer, byte playerID, [MaybeNullWhen(false)] out Player player)
        {
            m_Players.TryGetValue(playerID, out player);
            return player != null && player.m_Peer.Equals(peer);
        }

        public bool CheckPeerAndPlayerID(Peer peer, byte playerID)
        {
            m_Players.TryGetValue(playerID, out Player? player);
            return player != null && player.m_Peer.Equals(peer);
        }

        public byte FindOrCreateGroup(ulong loginKey, bool autoTeam)
        {
            Group? groupWithLoginKey = m_Groups.Values.FirstOrDefault(g => g.m_GroupLoginKey == loginKey);
            if (groupWithLoginKey != null)
            {
                return groupWithLoginKey.m_GroupIndex;
            }

            if (autoTeam)
            {
                Group? autoFillGroup = m_Groups.Values.FirstOrDefault(g => g.m_ShouldAutoFill && g.m_PlayerIDs.Count() < g.m_MaxPlayers);
                if (autoFillGroup != null)
                {
                    return autoFillGroup.m_GroupIndex;
                }
            }

            byte groupIndex = m_Groups.Keys.Count == 0 ? (byte)0 : (byte)(m_Groups.Keys.Max() + 1);
            m_Groups.Add(groupIndex, new Group(autoTeam, loginKey, groupIndex));
            return groupIndex;


        }
    }
}