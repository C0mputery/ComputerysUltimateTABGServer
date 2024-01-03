using ComputerysUltimateTABGServer.DataTypes.Items;
using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Packets;
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
            // This is gross.
            player = m_Players.Values.FirstOrDefault(p => p.m_Peer.Equals(peer));
            if (player != null && default(Player) != player)
            {
                return true;
            }
            return false;
        }
        public bool TryToGetPlayer(byte playerID, [MaybeNullWhen(false)] out Player player)
        {
            if (m_Players.TryGetValue(playerID, out player) && player != null)
            {
                return true;
            }
            return false;
        }
        public bool TryToGetPlayer(Peer peer, byte playerID, [MaybeNullWhen(false)] out Player player)
        {
            if (m_Players.TryGetValue(playerID, out player) && player != null && player.m_Peer.Equals(peer))
            {
                return true;
            }
            return false;
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

        public void GivePlayerLoot(Item item, Player player)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write((ushort)1); // How many items we are giving.
                binaryWriter.Write(item.WeaponIndex);
                binaryWriter.Write(item.Quantity);
                binaryWriter.Write(0); // Not sure what this is as it is discarded by the client.
                PacketManager.SendPacketToPlayer(EventCode.PlayerLootRecieved, memoryStream.ToArray(), player, this);

            }
        }
        public void GivePlayerLoot(Item[] items, Player player)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write((ushort)items.Length);
                foreach (Item item in items)
                {
                    binaryWriter.Write(item.WeaponIndex);
                    binaryWriter.Write(item.Quantity);
                }
                binaryWriter.Write(0); // Not sure what this is as it is discarded by the client.
                PacketManager.SendPacketToPlayer(EventCode.PlayerLootRecieved, memoryStream.ToArray(), player, this);
            }
        }
    }
}