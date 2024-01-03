using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void GearChangePacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            using (MemoryStream receivedPacketMemoryStream = new MemoryStream(receivedPacketRaw))
            using (BinaryReader receivedPacketBinaryReader = new BinaryReader(receivedPacketMemoryStream))
            {
                if (!room.TryToGetPlayer(peer, receivedPacketBinaryReader.ReadByte(), out Player? player)) { return; }

                int gearLenght = receivedPacketBinaryReader.ReadInt32();
                player.m_GearData = new int[gearLenght];
                for (int i = 0; i < gearLenght; i++)
                {
                    player.m_GearData[i] = receivedPacketBinaryReader.ReadInt32();
                }
                PacketManager.SendPacketToAllPlayersExcept(EventCode.GearChange, receivedPacketRaw, player, room);
            }
        }
    }
}
