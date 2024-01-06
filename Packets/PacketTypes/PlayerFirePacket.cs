using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void PlayerFirePacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            using (MemoryStream receivedPacketMemoryStream = new MemoryStream(receivedPacketRaw))
            using (BinaryReader receivedPacketBinaryReader = new BinaryReader(receivedPacketMemoryStream))
            {
                byte playerIndex = receivedPacketBinaryReader.ReadByte();
                FiringMode firingMode = (FiringMode)receivedPacketBinaryReader.ReadByte();
                int ammoType = receivedPacketBinaryReader.ReadInt32();
                if (receivedPacketRaw.Length - 6 > 0)
                {
                }
            }
        }
    }
}