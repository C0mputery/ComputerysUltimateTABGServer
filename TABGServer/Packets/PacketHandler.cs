using ENet;
using System.Collections.Frozen;
using TABGCommunityServer.ServerData;

namespace TABGCommunityServer.Packets
{
    public interface IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room);
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
                    packetHandler.Handle(peer, buffer, binaryReader, room);
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

        public static void BroadcastPacket(EventCode eventCode, byte[] playerData, bool unused, Room room)
        {
            foreach (var player in room.Players)
            {
                player.Value.PendingBroadcastPackets.Add(new Packet(eventCode, playerData));
            }
        }
    }
}