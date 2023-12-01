using ENet;
using TABGCommunityServer.Packets;
using TABGCommunityServer.ServerData;

namespace TABGCommunityServer.Rooms
{
    public static class RoomManager
    {
        public static List<Room> Rooms { get; private set; } = new List<Room>();

        public static void MakeRoom()
        {
            Rooms.Add(new Room(9997, 100));
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
                case EventType.None:
                    break;

                case EventType.Connect:
                    Console.WriteLine("Client connected - ID: " + room.enetEvent.Peer.ID + ", IP: " + room.enetEvent.Peer.IP);
                    break;

                case EventType.Disconnect:
                    Console.WriteLine("Client disconnected - ID: " + room.enetEvent.Peer.ID + ", IP: " + room.enetEvent.Peer.IP);
                    break;

                case EventType.Timeout:
                    Console.WriteLine("Client timeout - ID: " + room.enetEvent.Peer.ID + ", IP: " + room.enetEvent.Peer.IP);
                    break;

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
    }
}
