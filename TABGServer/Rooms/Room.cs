using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABGCommunityServer.Packets;

namespace TABGCommunityServer.ServerData
{
    public class Room
    {
        public Dictionary<byte, Player> Players = new Dictionary<byte, Player>();
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


        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
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


        public Room()
        {
        }
    }
}
