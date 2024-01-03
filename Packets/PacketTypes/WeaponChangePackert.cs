using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void WeaponChangePackert(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            if (room.CheckPeerAndPlayerID(peer, receivedPacketRaw[0]) {
                PacketManager.SendPacketToAllPlayers(EventCode.WeaponChanged, receivedPacketRaw, room);
            }
        }
    }
}