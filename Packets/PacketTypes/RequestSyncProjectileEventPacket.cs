using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.IO;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void RequestSyncProjectileEventPacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            using (MemoryStream receivedPacketMemoryStream = new MemoryStream(receivedPacketRaw))
            using (BinaryReader receivedPacketBinaryReader = new BinaryReader(receivedPacketMemoryStream))
            {
                int syncID = receivedPacketBinaryReader.ReadInt32();
                bool removed = receivedPacketBinaryReader.ReadBoolean();
                bool everyone = receivedPacketBinaryReader.ReadBoolean();
                bool includeSelf = receivedPacketBinaryReader.ReadBoolean();
                bool isStatic = receivedPacketBinaryReader.ReadBoolean();

                if (everyone) // so eventually we will have a chunk system 
                {
                    if (includeSelf)
                    {
                        PacketManager.SendPacketToAllPlayers(EventCode.SyncProjectileEvent, receivedPacketRaw, room);
                    }
                    else
                    {
                        PacketManager.SendPacketToAllPeersExcept(EventCode.SyncProjectileEvent, receivedPacketRaw, peer, room);
                    }
                }
                else
                {
                    if (includeSelf)
                    {
                        PacketManager.SendPacketToAllPlayers(EventCode.SyncProjectileEvent, receivedPacketRaw, room);
                    }
                    else
                    {
                        PacketManager.SendPacketToAllPeersExcept(EventCode.SyncProjectileEvent, receivedPacketRaw, peer, room);
                    }
                }

            }
        }
    }
}