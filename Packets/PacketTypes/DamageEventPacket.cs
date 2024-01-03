using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using Landfall.Network;
using System.Numerics;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void DamageEventPacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            using (MemoryStream receivedPacketMemoryStream = new MemoryStream(receivedPacketRaw))
            using (BinaryReader receivedPacketBinaryReader = new BinaryReader(receivedPacketMemoryStream))
            {
                if (!room.TryToGetPlayer(peer, receivedPacketBinaryReader.ReadByte(), out Player? AttackingPlayer)) { return; }

                if (!room.TryToGetPlayer(receivedPacketBinaryReader.ReadByte(), out Player? victimPlayer)) { return; }

                float heathToSetToo = receivedPacketBinaryReader.ReadSingle();
                float directionX = receivedPacketBinaryReader.ReadSingle();
                float directionY = receivedPacketBinaryReader.ReadSingle();
                float directionZ = receivedPacketBinaryReader.ReadSingle();
                bool ifRigExists = receivedPacketBinaryReader.ReadBoolean();

                float forceDirectionX = 0;
                float forceDirectionY = 0;
                float forceDirectionZ = 0;
                byte rigIndex = byte.MaxValue;
                byte forceMode = 0;

                bool dontReturnToSender = false; // This will be set to true if self damage is being dealt. and at some other point i think.
                if (ifRigExists)
                {
                    forceDirectionX = receivedPacketBinaryReader.ReadSingle();
                    forceDirectionY = receivedPacketBinaryReader.ReadSingle();
                    forceDirectionZ = receivedPacketBinaryReader.ReadSingle();
                    rigIndex = receivedPacketBinaryReader.ReadByte();
                    forceMode = receivedPacketBinaryReader.ReadByte();
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
                    // So the client checks a byte for if the second bit is set, if it is then it will update a rig
                    // so I think the code should look something like this
                    if (ifRigExists)
                    {
                        byte DumbByteThatThisDumbGameUses = 0;
                        DumbByteThatThisDumbGameUses |= (byte)(1 << 1); // Set the 2nd bit to 1
                        binaryWriter.Write(DumbByteThatThisDumbGameUses);
                    }
                    else { binaryWriter.Write((byte)0); }
                    binaryWriter.Write(NetworkOptimizationHelper.OptimizeDirection(new Vector3(forceDirectionX, forceDirectionY, forceDirectionZ)));
                    if (ifRigExists)
                    {
                        binaryWriter.Write(forceDirectionX);
                        binaryWriter.Write(forceDirectionY);
                        binaryWriter.Write(forceDirectionZ);
                        binaryWriter.Write(rigIndex);
                        binaryWriter.Write(forceMode);
                    }
                }

                if (dontReturnToSender) { PacketManager.SendPacketToAllPlayersExcept(EventCode.PlayerDamaged, receivedPacketRaw, AttackingPlayer, room); }
                else { PacketManager.SendPacketToAllPlayers(EventCode.PlayerDamaged, receivedPacketRaw, room); }
            }
        }
    }
}