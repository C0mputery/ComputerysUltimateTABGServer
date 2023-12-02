using ENet;
using System.Collections.Frozen;
using TABGCommunityServer.Packets;
using TABGCommunityServer.Packets.PacketTypes;

namespace TABGCommunityServer.Rooms
{
    public partial class Room
    {
        public delegate void HandlePacketDelegate();

        public readonly FrozenDictionary<EventCode, IPacket> packetHandlers = new Dictionary<EventCode, IPacket>
        {
            { EventCode.RoomInit, new RoomInitPacket() },
        }.ToFrozenDictionary();

        public string m_RoomName;
        public Host enetServer;
        public Address enetAddress;
        public Event enetEvent;

        public Room(ushort Port, int maxClients, string roomName)
        {
            m_RoomName = roomName ?? ""; // This is too make sure the m_roomName is not null.
            enetServer = new Host();
            enetAddress = new Address() { Port = Port };
            enetServer.Create(enetAddress, maxClients);
        }
    }
}
