using ENet;
using System.Collections.Frozen;

namespace TABGCommunityServer.Packets
{
    public interface IPacketHandler
    {
        void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader);
    }

    internal static class PacketHandler
    {
        private static readonly FrozenDictionary<EventCode, IPacketHandler> packetHandlers = new Dictionary<EventCode, IPacketHandler>
        {
            { EventCode.RoomInit, new RoomInitPacketHandler() },
            { EventCode.ChatMessage, new ChatMessagePacketHandler() },
            { EventCode.RequestItemThrow, new RequestItemThrowPacketHandler() },
            { EventCode.RequestItemDrop, new RequestItemDropPacketHandler() },
            { EventCode.RequestWeaponPickUp, new RequestWeaponPickUpPacketHandler() },
            { EventCode.PlayerUpdate, new PlayerUpdatePacketHandler() },
            { EventCode.WeaponChange, new WeaponChangePacketHandler() },
            { EventCode.PlayerFire, new PlayerFirePacketHandler() },
            { EventCode.RequestSyncProjectileEvent, new RequestSyncProjectileEventPacketHandler() },
            { EventCode.RequestAirplaneDrop, new RequestAirplaneDropPacketHandler() },
            { EventCode.DamageEvent, new DamageEventPacketHandler() },
            { EventCode.RequestBlessing, new RequestBlessingPacketHandler() },
            { EventCode.RequestHealthState, new RequestHealthStatePacketHandler() },
        }.ToFrozenDictionary();

        public static void Handle(Peer peer, EventCode code, byte[] buffer)
        {
            if ((code != EventCode.TABGPing) && (code != EventCode.PlayerUpdate))
            {
                Console.WriteLine("Handling packet: " + code.ToString());
            }

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            using (BinaryReader binaryReader = new BinaryReader(memoryStream))
            {
                if (packetHandlers.TryGetValue(code, out IPacketHandler? packetHandler))
                {
                    packetHandler.Handle(peer, buffer, binaryReader);
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

        public static void BroadcastPacket(EventCode eventCode, byte[] playerData, bool unused)
        {
            foreach (var player in PlayerConcurencyHandler.Players)
            {
                player.Value.PendingBroadcastPackets.Add(new Packet(eventCode, playerData));
            }
        }
    }
}