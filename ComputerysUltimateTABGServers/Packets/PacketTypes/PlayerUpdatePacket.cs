using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.Packets.PacketTypes
{
    public struct PlayerUpdatePacket : IPacket
    {
        public void Handle(Peer peer, BinaryReader receivedPacketData, Room room)
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
