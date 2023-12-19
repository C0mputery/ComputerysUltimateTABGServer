using ENet;
using System.Collections.Frozen;
using ComputerysUltimateTABGServer.Packets;

namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room : IDisposable
    {
        public static readonly FrozenDictionary<EventCode, PacketHandlerDelegate> PacketHandlers = new Dictionary<EventCode, PacketHandlerDelegate>
        {
            { EventCode.RoomInit, PacketTypes.RoomInitPacket },
            { EventCode.RequestWorldState, PacketTypes.RequestWorldStatePacket },
            { EventCode.PlayerUpdate, PacketTypes.PlayerUpdatePacket },
        }.ToFrozenDictionary();

        public string m_RoomName;
        public Host m_EnetServer;
        public Address m_EnetAddress;
        public Event m_EnetEvent;
        public int m_MaxClients;

        public bool m_ShouldEndRoom = false;
        public Task? Task = null;

        public double m_DelayBetweenTicks = 16;
        public DateTime m_LastTickTime = DateTime.Now;

        public Room(ushort Port, int maxClients, string roomName, double delayBetweenTicks)
        {
            m_RoomName = roomName ?? "Unnamed Room";
            m_EnetServer = new Host();
            m_EnetAddress = new Address() { Port = Port };
            m_EnetServer.Create(m_EnetAddress, maxClients);
            m_MaxClients = maxClients;
            m_DelayBetweenTicks = delayBetweenTicks;
        }

        // TODO: Make this actually dispose of the room.
        // Do not disspose of the room task, the garbage collector will do that for us.
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
