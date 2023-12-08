using ENet;
using System.Collections.Frozen;
using ComputerysUltimateTABGServers.Packets;
using ComputerysUltimateTABGServers.Packets.PacketTypes;

namespace ComputerysUltimateTABGServers.Rooms
{
    public partial class Room
    {
        public delegate void HandlePacketDelegate();

        public readonly FrozenDictionary<EventCode, IPacket> m_PacketHandlers = new Dictionary<EventCode, IPacket>
        {
            { EventCode.RoomInit, new RoomInitPacket() },
        }.ToFrozenDictionary();

        public string m_RoomName;
        public Host m_EnetServer;
        public Address m_EnetAddress;
        public Event m_EnetEvent;
        public int m_MaxClients;

        public Room(ushort Port, int maxClients, string roomName)
        {
            m_RoomName = roomName ?? ""; // This is too make sure the m_roomName is not null.
            m_EnetServer = new Host();
            m_EnetAddress = new Address() { Port = Port };
            m_EnetServer.Create(m_EnetAddress, maxClients);
            m_MaxClients = maxClients;
        }
    }
}
