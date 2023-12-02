using ENet;
using TABGCommunityServer.MiscDataTypes;
using TABGCommunityServer.Rooms;

namespace TABGCommunityServer.Packets
{
    public interface IPacketHandler
    {
        public void Handle(Peer peer, int bufferLength, BinaryReader binaryReader, Room room);
    }

    public static class PacketHandler
    {
        public static void Handle(Peer peer, EventCode code, byte[] buffer, Room room)
        {
            if ((code != EventCode.TABGPing) && (code != EventCode.PlayerUpdate))
            {
                Console.WriteLine("Handling packet: " + code.ToString());
            }

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            using (BinaryReader binaryReader = new BinaryReader(memoryStream))
            {
                if (room.packetHandlers.TryGetValue(code, out IPacketHandler? packetHandler))
                {
                    packetHandler.Handle(peer, buffer.Length, binaryReader, room);
                }
            }
        }

        public static void SendMessageToPeer(Peer peer, EventCode code, byte[] buffer, bool reliable)
        {
            byte[] array = new byte[buffer.Length + 1];
            array[0] = (byte)code;
            Array.Copy(buffer, 0, array, 1, buffer.Length);

            ENet.Packet packet = default;
            packet.Create(array, reliable ? PacketFlags.Reliable : PacketFlags.None);

            peer.Send(0, ref packet);
        }

        public static void BroadcastPacket(EventCode eventCode, byte[] playerData, Room room)
        {
            foreach (Player player in room.players.Values)
            {
                player.PendingBroadcastPackets.Add(new TabgPacket(eventCode, playerData));
            }
        }
    }
}