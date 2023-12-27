using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.Text;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void RoomInitPacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            using (MemoryStream receivedPacketMemoryStream = new MemoryStream(receivedPacketRaw))
            using (BinaryReader receivedPacketBinaryReader = new BinaryReader(receivedPacketMemoryStream))
            {
                string playerName = receivedPacketBinaryReader.ReadString();
                playerName = string.IsNullOrEmpty(playerName) ? "Unnamed" : playerName;
                string gravestoneText = receivedPacketBinaryReader.ReadString();
                gravestoneText = string.IsNullOrEmpty(gravestoneText) ? "No Gravestone Text" : gravestoneText;

                ulong loginKey = receivedPacketBinaryReader.ReadUInt64();
                bool isSquadHost = receivedPacketBinaryReader.ReadBoolean();
                byte numberOfSquadMembers = receivedPacketBinaryReader.ReadByte();

                int[] gearData = new int[receivedPacketBinaryReader.ReadInt32()];
                for (int i = 0; i < gearData.Length; i++) { gearData[i] = receivedPacketBinaryReader.ReadInt32(); }

                string steamTicket = receivedPacketBinaryReader.ReadString();
                receivedPacketBinaryReader.ReadBoolean(); // This is always false? Could possibly be for rejoining.
                string playfabID = receivedPacketBinaryReader.ReadString();
                int jsonWebTokenLength = receivedPacketBinaryReader.ReadInt32();
                receivedPacketBinaryReader.ReadBytes(jsonWebTokenLength); // this would be a jsonWebToken, this is part of epic EOSSDK so we don't need it.
                int productidLength = receivedPacketBinaryReader.ReadInt32();
                receivedPacketBinaryReader.ReadBytes(productidLength); // this would be a productid, this is part of epic EOSSDK so we don't need it.
                int color = receivedPacketBinaryReader.ReadInt32();
                bool shouldAutoFillSquad = receivedPacketBinaryReader.ReadBoolean();

                // I am not sure how I could handle the unityAuthPlayerID being null and also obtain the server password, this needs to be looked into.
                string unityAuthPlayerId = string.Empty;
                if (receivedPacketBinaryReader.PeekChar() != -1) { unityAuthPlayerId = receivedPacketBinaryReader.ReadString(); }

                // Tecnically this is handled by the server list, so we should be able to just ignore this.
                string serverPassword = string.Empty;
                if (receivedPacketBinaryReader.PeekChar() != -1) { serverPassword = receivedPacketBinaryReader.ReadString(); }

                byte groupIndex = room.FindOrCreateGroup(loginKey, shouldAutoFillSquad);
                Player player = new Player(peer, playerName, groupIndex, gearData, playfabID, color);
                room.AddPlayer(player);

                byte[] playerNameUTF8 = Encoding.UTF8.GetBytes(playerName);
                using (MemoryStream memoryStream = new MemoryStream())
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write((byte)peer.ID);
                    binaryWriter.Write(groupIndex);
                    binaryWriter.Write(playerNameUTF8.Length);
                    binaryWriter.Write(playerNameUTF8);
                    binaryWriter.Write(gearData.Length);
                    for (int i = 0; i < gearData.Length; i++)
                    {
                        binaryWriter.Write(gearData[i]);
                    }
                    binaryWriter.Write(player.m_IsDev);
                    binaryWriter.Write(color);

                    PacketManager.SendPacketToAllPlayers(EventCode.Login, memoryStream.ToArray(), room);
                }

                using (MemoryStream memoryStream = new MemoryStream())
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write((byte)ServerResponse.Accepted);
                    binaryWriter.Write((byte)room.m_GameMode);
                    binaryWriter.Write((byte)room.m_MatchMode);
                    binaryWriter.Write(peer.ID);
                    binaryWriter.Write(player.m_GroupIndex);
                    binaryWriter.Write(playerNameUTF8.Length);
                    binaryWriter.Write(playerNameUTF8);

                    PacketManager.SendPacketToPlayer(EventCode.RoomInitRequestResponse, memoryStream.ToArray(), player, room);
                }
            }
        }
    }
}