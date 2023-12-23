using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void GearChangePacket(Peer peer, byte[] receivedPacketRaw, BinaryReader receivedPacketBinaryReader, Room room)
        {
                if (!room.TryToGetPlayer(receivedPacketBinaryReader.ReadByte(), out Player? player) || player == null) { return; }
                int gearLenght = receivedPacketBinaryReader.ReadInt32();
                for (int i = 0; i < gearLenght; i++)
                {
                    player.m_GearData[i] = receivedPacketBinaryReader.ReadInt32(); // This may cause an error if we have more gear than the player array size, but this should never happen.
                }
                PacketHandler.SendPacketToAllPlayersExcept(EventCode.GearChange, receivedPacketRaw, player, room);
        }
    }
}
