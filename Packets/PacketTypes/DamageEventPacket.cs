using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void DamageEventPacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            using (MemoryStream receivedPacketMemoryStream = new MemoryStream(receivedPacketRaw))
            using (BinaryReader receivedPacketBinaryReader = new BinaryReader(receivedPacketMemoryStream))
            {
                if (!room.TryToGetPlayer(receivedPacketBinaryReader.ReadByte(), out Player? AttackingPlayer)) { return; }
                if (AttackingPlayer.m_PlayerID != peer.ID) { return; }

                if (!room.TryToGetPlayer(receivedPacketBinaryReader.ReadByte(), out Player? victimPlayer)) { return; }

                float heathToSetToo = receivedPacketBinaryReader.ReadSingle();
                float directionX = receivedPacketBinaryReader.ReadSingle();
                float directionY = receivedPacketBinaryReader.ReadSingle();
                float directionZ = receivedPacketBinaryReader.ReadSingle();
                bool rigIndexIsNotMaxValue = receivedPacketBinaryReader.ReadBoolean();
                bool dontReturnToSender = false; // This will be set to true if self damage is being dealt.
                if (rigIndexIsNotMaxValue)
                {
                    float forceDirectionX = receivedPacketBinaryReader.ReadSingle();
                    float forceDirectionY = receivedPacketBinaryReader.ReadSingle();
                    float forceDirectionZ = receivedPacketBinaryReader.ReadSingle();
                    byte rigIndex = receivedPacketBinaryReader.ReadByte();
                    byte forceMode = receivedPacketBinaryReader.ReadByte();
                }
                else
                {
                    dontReturnToSender = receivedPacketBinaryReader.ReadBoolean();
                }

                using (MemoryStream memoryStream = new MemoryStream())
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(victimPlayer.m_PlayerID);
                    binaryWriter.Write(AttackingPlayer.m_PlayerID);
                    binaryWriter.Write(victimPlayer.m_Health);
                
                }

                if (dontReturnToSender) { PacketManager.SendPacketToAllPlayersExcept(EventCode.PlayerDamaged, receivedPacketRaw, AttackingPlayer, room); }
                else { PacketManager.SendPacketToAllPlayers(EventCode.PlayerDamaged, receivedPacketRaw, room); }
            }
        }
    }
}