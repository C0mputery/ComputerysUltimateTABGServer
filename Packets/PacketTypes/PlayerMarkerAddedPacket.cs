using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void PlayerMarkerAddedPacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            if (room.TryToGetPlayer(peer, receivedPacketRaw[0], out Player? player) && room.m_Groups.TryGetValue(player.m_GroupIndex, out Group? group))
            {
                PacketManager.SendPacketToPlayers(EventCode.PlayerMarkerEvent, receivedPacketRaw, group.m_PlayerIDs.ToArray(), room);
            }
        }
    }
}
