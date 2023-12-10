using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void PlayerUpdatePacket(Peer peer, BinaryReader receivedPacketData, Room room)
        {
            room.TryToGetPlayer(receivedPacketData.ReadByte(), out Player? player);
            if (player == null) { return; }
            player.m_Position.X = receivedPacketData.ReadSingle();
            player.m_Position.Y = receivedPacketData.ReadSingle();
            player.m_Position.Z = receivedPacketData.ReadSingle();
            player.m_Rotation.X = receivedPacketData.ReadSingle();
            player.m_Rotation.Y = receivedPacketData.ReadSingle();
            player.m_AimingDownSights = receivedPacketData.ReadBoolean();
            player.m_MovmentDirction = receivedPacketData.ReadBytes(3);
            player.m_MovementFlags = receivedPacketData.ReadByte();
        }
    }
}
