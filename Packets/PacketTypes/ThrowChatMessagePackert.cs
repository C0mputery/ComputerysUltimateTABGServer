using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void ThrowChatMessagePacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {  
            if (!room.TryToGetPlayer(receivedPacketRaw[0], out Player? player)) { return; }
            byte[] TextBytesAndLength = receivedPacketRaw[1..];
            //string chatText = Encoding.Unicode.GetString(TextBytesAndLength[1..]);

            using (MemoryStream responseMemoryStream = new MemoryStream())
            using (BinaryWriter responseBinaryWriter = new BinaryWriter(responseMemoryStream))
            {
                responseBinaryWriter.Write(player.m_PlayerID);
                responseBinaryWriter.Write(player.m_Position.X);
                responseBinaryWriter.Write(player.m_Position.Y);
                responseBinaryWriter.Write(player.m_Position.Z);
                responseBinaryWriter.Write(player.m_Rotation.X);
                responseBinaryWriter.Write(player.m_Rotation.Y);
                responseBinaryWriter.Write(TextBytesAndLength);
                PacketManager.SendPacketToAllPlayers(EventCode.ThrowChatMessage, responseMemoryStream.ToArray(), room);
            }
        }
    }
}