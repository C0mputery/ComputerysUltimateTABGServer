using ENet;
using TABGCommunityServer.MiscDataTypes;
using TABGCommunityServer.Rooms;

namespace TABGCommunityServer.Packets
{
    public interface IPacket
    {
        public void Handle(byte peerID, BinaryReader receivedPacketData, Room room);
    }

    public static class PacketHandler
    {
        public static void Handle(EventCode eventCode, byte peerID, byte[] packetData, Room room)
        {
            if ((eventCode != EventCode.TABGPing) && (eventCode != EventCode.PlayerUpdate))
            {
                Console.WriteLine("Handling packet: " + eventCode.ToString());
            }

            using (MemoryStream packetDataMemoryStream = new MemoryStream(packetData))
            using (BinaryReader packetDataBinaryReader = new BinaryReader(packetDataMemoryStream))
            {
                if (room.packetHandlers.TryGetValue(eventCode, out IPacket? packetHandler))
                {
                    packetHandler.Handle(peerID, packetDataBinaryReader, room);
                }
            }
        }

        public static void SendPacketToPlayer(EventCode eventCode, byte[] packetData, Player recipent, Room room)
        {
            byte[] packetByteArray = new byte[packetData.Length + 1];
            packetByteArray[0] = (byte)eventCode;
            Array.Copy(packetData, 0, packetByteArray, 1, packetData.Length);
            Packet packet = default(Packet);
            packet.Create(packetByteArray, PacketFlags.Reliable);
            recipent.m_Peer.Send(0, ref packet);
        }

        public static void SendPacketToPlayers(EventCode eventCode, byte[] packetData, Player[] recipents, Room room)
        {
            byte[] packetByteArray = new byte[packetData.Length + 1];
            packetByteArray[0] = (byte)eventCode;
            Array.Copy(packetData, 0, packetByteArray, 1, packetData.Length);
            Packet packet = default(Packet);
            packet.Create(packetByteArray, PacketFlags.Reliable);
            Peer[] peersArray = recipents.Select(player => player.m_Peer).ToArray();
            room.enetServer.Broadcast(0, ref packet, peersArray);
        }

        public static void SendPacketAllPlayers(EventCode eventCode, byte[] packetData, Room room)
        {
            SendPacketToPlayers(eventCode, packetData, room.players.Values.ToArray(), room);
        }
    }
}