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
                if (!room.TryToGetPlayer(receivedPacketBinaryReader.ReadByte(), out Player? player)) { return; }
                if (player.m_PlayerID != peer.ID) { return; } // This may need to be changed as I am not sure if we are able to keep the playerID the same as the peerID.

                int gearLenght = receivedPacketBinaryReader.ReadInt32();
                for (int i = 0; i < gearLenght; i++)
                {
                    player.m_GearData[i] = receivedPacketBinaryReader.ReadInt32(); // This may cause an error if we have more gear than the player array size, but this should never happen.
                }
                PacketManager.SendPacketToAllPlayersExcept(EventCode.GearChange, receivedPacketRaw, player, room);
            }
        }
    }
}
