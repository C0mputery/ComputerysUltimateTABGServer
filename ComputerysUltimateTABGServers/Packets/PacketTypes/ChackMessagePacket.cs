using ComputerysUltimateTABGServer.Interface.Logging;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void ChatMessagePacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            byte PlayerID = receivedPacketRaw[0];
            byte MessageLength = receivedPacketRaw[1];
            string Message = System.Text.Encoding.UTF8.GetString(receivedPacketRaw, 2, MessageLength);
            // Add some adimn commands here
            PacketHandler.SendPacketToAllPlayers(EventCode.ChatMessage, receivedPacketRaw, room);
        }
    }
}