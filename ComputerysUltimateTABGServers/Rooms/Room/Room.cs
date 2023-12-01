using ENet;
using System.Collections.Frozen;
using TABGCommunityServer.Packets;
using TABGCommunityServer.Packets.PacketTypes;

namespace TABGCommunityServer.Rooms
{
    public partial class Room
    {
        public readonly FrozenDictionary<EventCode, IPacketHandler> packetHandlers = new Dictionary<EventCode, IPacketHandler>
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

        public int roomID;
        public Host enetServer;
        public Address enetAddress;
        public Event enetEvent;

        public Room(ushort Port, int maxClients)
        {
            enetServer = new Host();
            enetAddress = new Address() { Port = Port };
            enetServer.Create(enetAddress, maxClients);
        }
    }
}
