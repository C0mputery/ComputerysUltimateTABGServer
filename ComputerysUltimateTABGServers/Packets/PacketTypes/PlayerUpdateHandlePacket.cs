using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void PlayerUpdatePacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            using (MemoryStream receivedPacketMemoryStream = new MemoryStream(receivedPacketRaw))
            using (BinaryReader receivedPacketBinaryReader = new BinaryReader(receivedPacketMemoryStream))
            {
                if (!room.TryToGetPlayer(receivedPacketBinaryReader.ReadByte(), out Player? player) || player == null) { return; }
                player.m_Position.X = receivedPacketBinaryReader.ReadSingle();
                player.m_Position.Y = receivedPacketBinaryReader.ReadSingle();
                player.m_Position.Z = receivedPacketBinaryReader.ReadSingle();
                player.m_Rotation.X = receivedPacketBinaryReader.ReadSingle();
                player.m_Rotation.Y = receivedPacketBinaryReader.ReadSingle();
                player.m_AimingDownSights = receivedPacketBinaryReader.ReadBoolean();
                player.m_MovmentDirction = receivedPacketBinaryReader.ReadBytes(3);
                player.m_MovementFlags = receivedPacketBinaryReader.ReadByte();
            }
        }
    }
}
