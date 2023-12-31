using ENet;
using System.Collections.Frozen;
using ComputerysUltimateTABGServer.Packets;

namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room
    {
        public string m_RoomName;
        public Host m_EnetServer;
        public Address m_EnetAddress;
        public Event m_EnetEvent;
        public int m_MaxClients;

        public Task Task = null!;
        public bool m_ShouldEndRoom = false;

        public double m_DelayBetweenTicks = 16;          
        public TimeSpan m_ElapsedTime = TimeSpan.Zero;
        public DateTime m_LastTickTime = DateTime.Now;
        public float m_TickUpdateRange = 500;

        // This constuctor should probably updated.
        public Room(ushort Port, int maxClients, string roomName, double delayBetweenTicks)
        {
            m_RoomName = roomName ?? "Unnamed Room";
            m_EnetServer = new Host();
            m_EnetAddress = new Address() { Port = Port };
            m_EnetServer.Create(m_EnetAddress, maxClients);
            m_MaxClients = maxClients;
            m_DelayBetweenTicks = delayBetweenTicks;
        }
    }
}
