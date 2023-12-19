using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.Text;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void RoomInitPacket(Peer peer, BinaryReader receivedPacketData, Room room)
        {
            string playerName = receivedPacketData.ReadString();
            playerName = string.IsNullOrEmpty(playerName) ? "Unnamed" : playerName;
            string gravestoneText = receivedPacketData.ReadString();
            gravestoneText = string.IsNullOrEmpty(gravestoneText) ? "No Gravestone Text" : gravestoneText;

            ulong loginKey = receivedPacketData.ReadUInt64();
            bool isSquadHost = receivedPacketData.ReadBoolean();
            byte numberOfSquadMembers = receivedPacketData.ReadByte();

            int[] gearData = new int[receivedPacketData.ReadInt32()];
            for (int i = 0; i < gearData.Length; i++) { gearData[i] = receivedPacketData.ReadInt32(); }

            string steamTicket = receivedPacketData.ReadString();
            receivedPacketData.ReadBoolean(); // This is always false? Could possibly be for rejoining.
            string playfabID = receivedPacketData.ReadString();
            int jsonWebTokenLength = receivedPacketData.ReadInt32();
            receivedPacketData.ReadBytes(jsonWebTokenLength); // this would be a jsonWebToken, this is part of epic EOSSDK so we don't need it.
            int productidLength = receivedPacketData.ReadInt32();
            receivedPacketData.ReadBytes(productidLength); // this would be a productid, this is part of epic EOSSDK so we don't need it.
            int color = receivedPacketData.ReadInt32();
            bool shouldAutoFillSquad = receivedPacketData.ReadBoolean();

            // I am not sure how I could handle the unityAuthPlayerID being null and also obtain the server password, this needs to be looked into.
            string unityAuthPlayerId = string.Empty;
            if (receivedPacketData.PeekChar() != -1) { unityAuthPlayerId = receivedPacketData.ReadString(); }

            // Tecnically this is handled by the server list, so we should be able to just ignore this.
            string serverPassword = string.Empty;
            if (receivedPacketData.PeekChar() != -1) { serverPassword = receivedPacketData.ReadString(); }

            byte groupIndex = room.FindOrCreateGroup(loginKey, shouldAutoFillSquad);
            Player player = new Player(peer, playerName, groupIndex, gearData, playfabID, color);
            room.AddPlayer(player);

            byte[] playerNameUTF8 = Encoding.UTF8.GetBytes(playerName);
            byte[] loginData;
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(peer.ID);
                binaryWriter.Write(groupIndex);
                binaryWriter.Write(playerNameUTF8.Length);
                binaryWriter.Write(playerNameUTF8);
                binaryWriter.Write(gearData.Length);
                for (int i = 0; i < gearData.Length; i++)
                {
                    binaryWriter.Write(gearData[i]);
                }
                binaryWriter.Write(false);
                binaryWriter.Write(color);

                loginData = memoryStream.ToArray();
            }
            PacketHandler.SendPacketToAllPlayers(EventCode.Login, loginData, room);

            byte[] initData;
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write((byte)1);
                binaryWriter.Write((byte)room.m_GameMode);
                binaryWriter.Write((byte)room.m_MatchMode);
                binaryWriter.Write(peer.ID);
                binaryWriter.Write(player.m_GroupIndex);
                binaryWriter.Write(playerNameUTF8.Length);
                binaryWriter.Write(playerNameUTF8);

                initData = memoryStream.ToArray();
            }
            PacketHandler.SendPacketToPlayer(EventCode.RoomInitRequestResponse, initData, player, room);
        }
    }
}