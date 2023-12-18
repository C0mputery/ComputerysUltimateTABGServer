using ComputerysUltimateTABGServer.Packets;
using ENet;
using System.Collections.Concurrent;

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

        /// <summary>
        /// This should only be called once before any other room update loops are started
        /// Is this technically because I wrote it wrong? Yes, but it works so I'm not changing it.
        /// If I wanted to fix it I would need to keep track of which rooms have update loops running and which don't feel like doing right now.
        /// </summary>
        internal static void StartAllRoomUpdateLoops()
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
            // This is diffrerent from the eNet example code, because the example code makes no god damn sense.
            // If Check Events returns an int that's not above zero, it means that m_EnetEvent is goint to be a default value.
            // Which means that it is redundant to check what type of event it is.
            // So rather than running throught the loop again than breaking out of it, we just stop.
            while (room.m_EnetServer.CheckEvents(out room.m_EnetEvent) >= 0)
            {
                // This line is straight from the eNet example code, I don't know why they do it this way but I'm not going to change it.
                // Idk if Service is checking a single packet or all packets in the queue, but I'm assuming it's all packets in the queue.
                // Since within all the example code it ends while loop, used to check packets.
                if (room.m_EnetServer.Service(15, out room.m_EnetEvent) <= 0) { break; }

                switch (room.m_EnetEvent.Type)
                {
                    //fortnite
                    case EventType.Receive:
                        byte[] enetPacket = new byte[room.m_EnetEvent.Packet.Length];
                        room.m_EnetEvent.Packet.CopyTo(enetPacket);

                        EventCode eventCode = (EventCode)enetPacket[0];
                        byte[] packetData = new byte[enetPacket.Length - 1];
                        Array.Copy(enetPacket, 1, packetData, 0, packetData.Length);

                        PacketHandler.Handle(eventCode, room.m_EnetEvent.Peer, packetData, room);

                        // Aparently it's unnecessary to dispose of packets that are not of the Receive type, so I'm not going to do it.
                        room.m_EnetEvent.Packet.Dispose();

                        break;
                }
            }


        }
    }
}
