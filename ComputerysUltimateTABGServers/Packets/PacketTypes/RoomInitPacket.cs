using ENet;
using System.Text;
using ComputerysUltimateTABGServers.Rooms;
using ComputerysUltimateTABGServers.MiscDataTypes;

namespace ComputerysUltimateTABGServers.Packets.PacketTypes
{
    public struct RoomInitPacket : IPacket
    {
        public void Handle(Peer peer, BinaryReader receivedPacketData, Room room)
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
            string serverPassword = string.Empty;
            if (receivedPacketData.PeekChar() != -1) { serverPassword = receivedPacketData.ReadString(); }

            byte groupIndex = room.JoinOrCreateGroup(peer, loginKey, shouldAutoFillSquad);
            Player player = new Player(peer, playerName, 0, gearData, playfabID, color);
            room.AddPlayer(player);

            /*byte[] playerNameUTF8 = Encoding.UTF8.GetBytes(playerName);
            byte[] buffer = new byte[15 + playerNameUTF8.Length + (4 * gearData.Length)];
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(tABGPlayerServer3.PlayerIndex);
                binaryWriter.Write(tABGPlayerServer3.GroupIndex);
                binaryWriter.Write(playerNameUTF8.Length);
                binaryWriter.Write(playerNameUTF8);
                binaryWriter.Write(gearData.Length);
                for (int i = 0; i < gearData.Length; i++)
                {
                    binaryWriter.Write(gearData[i]);
                }
                binaryWriter.Write(0);
                binaryWriter.Write(color);
            }*/

            /*byte newIndex = room.lastPlayerID++;
            byte[] sendByte = new byte[4 + 4 + 4 + 4 + 4 + 4 + room.m_RoomName.Length + 4];

            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
            {
                // accepted or not
                binaryWriterStream.Write((byte)ServerResponse.Accepted);
                // gamemode
                binaryWriterStream.Write((byte)GameMode.BattleRoyale);
                // client requires this, but it's useless
                binaryWriterStream.Write((byte)1);
                // player index
                binaryWriterStream.Write(newIndex);
                // group index
                binaryWriterStream.Write((byte)0);
                // useless
                binaryWriterStream.Write(1);
                // useless string (but using it to notify server of a custom server)
                binaryWriterStream.Write(Encoding.UTF8.GetBytes("CustomServer"));
            }

            Console.WriteLine("Sending request RESPONSE to client!");
            PacketHandler.SendMessageToPeer(peer, EventCode.RoomInitRequestResponse, sendByte, true);

            Console.WriteLine("Sending Login RESPONSE to client!");
            PacketHandler.SendMessageToPeer(peer, EventCode.Login, SendJoinMessageToServer(newIndex, playerName, gearData, room), true);

            foreach (KeyValuePair<byte, Player> player in room.players)
            {
                if (player.Key == newIndex)
                {
                    continue;
                }

                // broadcast to ALL players
                player.Value.PendingBroadcastPackets.Add(new TabgPacket(EventCode.Login, SendLoginMessageToServer(newIndex, playerName, gearData)));
            }

            PacketHandler.SendMessageToPeer(peer, EventCode.PlayerDead, room.SendNotification(0, "WELCOME - RUNNING COMMUNITY SERVER V1.TEST"), true);*/
        }
    }
}