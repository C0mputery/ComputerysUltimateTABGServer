using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void TabgPingPacket(Peer peer, byte[] receivedPacketRaw, BinaryReader receivedPacketBinaryReader, Room room)
        {
            // wow that's it?
            PacketHandler.SendPacketToPeer(EventCode.TABGPing, receivedPacketRaw, peer, room);
        }
    }
}