using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void TabgPingPacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            PacketManager.SendPacketToPeer(EventCode.TABGPing, receivedPacketRaw, peer, room);
        }
    }
}