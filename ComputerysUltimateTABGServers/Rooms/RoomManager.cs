using ComputerysUltimateTABGServer.Packets;
using ENet;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ComputerysUltimateTABGServer.Rooms
{
    public static class RoomManager
    {
        public static ConcurrentDictionary<ushort, Room> Rooms { get; private set; } = new ConcurrentDictionary<ushort, Room>();
        public static void MakeRoom(ushort port, int maxPlayers, string roomName)
        {
            Room room = new Room(port, maxPlayers, roomName);
            Rooms.TryAdd(port, room);
        }

        public static void EndAllRooms()
        {
            foreach (ushort room in Rooms.Keys)
            {
                EndRoom(room);
            }
        }
        public static void EndRoom(ushort roomPort)
        {
            Rooms.Remove(roomPort, out Room? room);
            if (room != null) { room.m_ShouldEndRoom = true; }
        }
        public static void EndRoom(Room room)
        {
            Rooms.Remove(Rooms.First(KeyValuePar => KeyValuePar.Value == room).Key, out Room? _);
        }

        public static void StartAllRoomUpdateLoops()
        {
            foreach (Room room in Rooms.Values)
            {
                StartRoomUpdateLoop(room);
            }
        }
        public static void StartRoomUpdateLoop(Room room)
        {
            Task.Run(() => RoomUpdateLoop(room));
        }

        private static void RoomUpdateLoop(Room room)
        {
            while (!room.m_ShouldEndRoom)
            {
                RoomUpdate(room);
            }
            room.m_EnetServer.Flush();
        }

        private static void RoomUpdate(Room room)
        {
            if (room.m_EnetServer.CheckEvents(out room.m_EnetEvent) >= 0 && room.m_EnetServer.Service(15, out room.m_EnetEvent) >= 0)
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

                        break;
                }
            }
            room.m_EnetEvent.Packet.Dispose();
        }
    }
}
