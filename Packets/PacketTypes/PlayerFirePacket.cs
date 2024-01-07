using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Interface.Logging;
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
                if (!room.TryToGetPlayer(peer, receivedPacketBinaryReader.ReadByte(), out Player? player)) { return; }

                FiringMode firingMode = (FiringMode)receivedPacketBinaryReader.ReadByte();
                int ammoType = receivedPacketBinaryReader.ReadInt32();
                CUTSLogger.Log("PlayerFirePacket: " + player.m_PlayerID + " " + firingMode + " " + ammoType, LogLevel.Info);
                if (firingMode.HasFlag(FiringMode.WantsToBeSynced))
                {
                    byte[] PlayerFireSyncReturn = new byte[4];
                    Array.Copy(receivedPacketRaw, receivedPacketRaw.Length - 4, PlayerFireSyncReturn, 0, 4);
                    PacketManager.SendPacketToPlayer(EventCode.PlayerFireSyncReturn, PlayerFireSyncReturn, player, room);
                }
                PacketManager.SendPacketToAllPlayersExcept(EventCode.PlayerFire, receivedPacketRaw, player, room);
            }
        }
    }
}