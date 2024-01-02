using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.Text;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void ThrowChatMessagePackert(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            using (MemoryStream memoryStream = new MemoryStream(receivedPacketRaw))
            using (BinaryReader binaryReader = new BinaryReader(memoryStream))
            {
                byte playerID = binaryReader.ReadByte();
                byte textLength = binaryReader.ReadByte();
                string chatText = Encoding.Unicode.GetString(binaryReader.ReadBytes(textLength));
            }
        }
    }
}