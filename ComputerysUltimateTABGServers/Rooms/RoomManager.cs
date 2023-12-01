using ENet;
using TABGCommunityServer.Packets;

namespace TABGCommunityServer.Rooms
{
    public static class RoomManager
    {
        public static List<Room> Rooms { get; private set; } = [];

        public static void MakeRoom(ushort port, int maxPlayers)
        {
            Room room = new(port, maxPlayers);
            Rooms.Add(room);
            RegisterOnServerList(room);
        }

        public static void UpdateRooms()
        {
            foreach (Room room in Rooms)
            {
                if (room.enetServer.CheckEvents(out room.enetEvent) >= 0 && room.enetServer.Service(15, out room.enetEvent) >= 0)
                {
                    UpdateRoom(room);
                }
            }
        }

        public static void CloseAllRooms()
        {
            foreach (Room room in Rooms)
            {
                room.enetServer.Flush();
            }
            Rooms.Clear();
        }

        private static void UpdateRoom(Room room)
        {
            switch (room.enetEvent.Type)
            {
                case EventType.Receive:
                    byte[] enetPacket = new byte[room.enetEvent.Packet.Length];
                    room.enetEvent.Packet.CopyTo(enetPacket);

                    EventCode code = (EventCode)enetPacket[0];
                    byte[] buffer = new byte[enetPacket.Length - 1];
                    Array.Copy(enetPacket, 1, buffer, 0, buffer.Length);

                    PacketHandler.Handle(room.enetEvent.Peer, code, buffer, room);

                    room.enetEvent.Packet.Dispose();
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
