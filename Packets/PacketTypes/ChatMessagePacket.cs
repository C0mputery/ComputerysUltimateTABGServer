using ComputerysUltimateTABGServer.Interface.Logging;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void ChatMessagePacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            PacketManager.SendPacketToAllPlayers(EventCode.ChatMessage, receivedPacketRaw, room);
        }
    }
}