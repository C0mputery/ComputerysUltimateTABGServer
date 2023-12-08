using ENet;
using ComputerysUltimateTABGServers.Packets;

namespace ComputerysUltimateTABGServers.Rooms
{
    public static class RoomManager
    {
        public static List<Room> Rooms { get; private set; } = [];

        public static void MakeRoom(ushort port, int maxPlayers)
        {
            Room room = new(port, maxPlayers, "testServer");
            RegisterOnServerList(room);
            Rooms.Add(room);
        }

        public static void UpdateRooms()
        {
            foreach (Room room in Rooms)
            {
                if (room.m_EnetServer.CheckEvents(out room.m_EnetEvent) >= 0 && room.m_EnetServer.Service(15, out room.m_EnetEvent) >= 0)
                {
                    UpdateRoom(room);
                }
            }
        }

        public static void CloseAllRooms()
        {
            foreach (Room room in Rooms)
            {
                room.m_EnetServer.Flush();
            }
            Rooms.Clear();
        }

        private static void UpdateRoom(Room room)
        {
            switch (room.m_EnetEvent.Type)
            {
                case EventType.Receive:
                    byte[] enetPacket = new byte[room.m_EnetEvent.Packet.Length];
                    room.m_EnetEvent.Packet.CopyTo(enetPacket);

                    EventCode eventCode = (EventCode)enetPacket[0];
                    byte[] packetData = new byte[enetPacket.Length - 1];
                    Array.Copy(enetPacket, 1, packetData, 0, packetData.Length);

                    PacketHandler.Handle(eventCode, room.m_EnetEvent.Peer, packetData, room);

                    room.m_EnetEvent.Packet.Dispose();
                    break;
            }
        }

        /// <summary>
        /// To be implemented
        /// </summary>
        private static Timer? serverListHeartbeat;

        /// <summary>
        /// To be implemented
        /// </summary>
        private static void RegisterOnServerList(Room room)
        {

        }

        /// <summary>
        /// To be implemented
        /// </summary>
        public static void StartServerListHeartbeat()
        {
            serverListHeartbeat = new Timer(ServerListHeartbeat, null, 0, 5000);
        }

        /// <summary>
        /// To be implemented
        /// </summary>
        private static void ServerListHeartbeat(object? state)
        {
            foreach (Room room in Rooms)
            {

            }
        }
    }
}
