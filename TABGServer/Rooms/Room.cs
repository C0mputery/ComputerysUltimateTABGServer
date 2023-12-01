using ENet;
using System.Collections.Frozen;
using TABGCommunityServer.DataTypes;
using TABGCommunityServer.Packets;

namespace TABGCommunityServer.ServerData
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

        public Dictionary<byte, Player> Players { get; private set; } = new Dictionary<byte, Player>();
        public byte LastID = 0;
        public void AddPlayer(Player player)
        {
            Players[player.Id] = player;
            LastID++;
        }
        public void RemovePlayer(Player player)
        {
            Players.Remove(player.Id);
        }

        public Dictionary<int, Item> Items { get; private set; } = new Dictionary<int, Item>();
        public int CurrentID = 0;
        public void SpawnItem(Item item)
        {
            Items[item.Id] = item;
            CurrentID++;
        }
        public void RemoveItem(Item item)
        {
            Items.Remove(item.Id);
        }

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
