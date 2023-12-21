using ComputerysUltimateTABGServer.Interface.Logging;
using ComputerysUltimateTABGServer.Packets;
using ComputerysUltimateTABGServer.Ticks;
using ENet;
using System.Collections.Concurrent;

namespace ComputerysUltimateTABGServer.Rooms
{
    public static class RoomManager
    {
        public static ConcurrentDictionary<ushort, Room> ActiveRooms { get; private set; } = new ConcurrentDictionary<ushort, Room>();
        public static void MakeRoom(ushort port, int maxPlayers, string roomName, double tickRate)
        {
            Room room = new Room(port, maxPlayers, roomName, 1000 / tickRate);
            room.Task = Task.Run(() => RoomUpdateLoop(room));
            if (!ActiveRooms.TryAdd(port, room)) { CUTSLogger.Log($"Failed to make room: {roomName}, on port: {port}", LogLevel.Error); room.m_ShouldEndRoom = true; return; }
            CUTSLogger.Log($"Made room: {roomName}, on port: {port}", LogLevel.Info);
        }

        public static void EndAllRooms()
        {
            foreach (Room room in ActiveRooms.Values) { EndRoom(room); }
        }
        public static void EndRoom(ushort roomPort)
        {
            ActiveRooms.TryGetValue(roomPort, out Room? room);
            if (room != null) { room.m_ShouldEndRoom = true; }
        }
        public static void EndRoom(Room room)
        {
            room.m_ShouldEndRoom = true;
        }

        private static void RoomUpdateLoop(Room room)
        {
            while (!room.m_ShouldEndRoom)
            {
                RoomUpdate(room);
            }
            RoomUpdateEnded(room);
        }
        private static void RoomUpdate(Room room)
        {
            RoomPackets(room);
            RoomTick(room);
        }
        private static void RoomUpdateEnded(Room room)
        {
            room.m_EnetServer.Flush();
            if (!ActiveRooms.TryRemove(room.m_EnetAddress.Port, out _))
            {
                CUTSLogger.Log($"Failed to remove room: {room.m_RoomName}, on port: {room.m_EnetAddress.Port}, running failsafe room removal!", LogLevel.Error);
                if (!ActiveRooms.TryRemove(ActiveRooms.First(KeyValuePar => KeyValuePar.Value == room).Key, out Room? _))
                {
                    CUTSLogger.Log($"Failed to remove room: {room.m_RoomName}, on port: {room.m_EnetAddress.Port}, even with failsafe room removal!", LogLevel.Error);
                    return;
                }
                return;
            }
            CUTSLogger.Log($"Ended room: {room.m_RoomName}, on port: {room.m_EnetAddress.Port}", LogLevel.Info);
        }

        private static void RoomPackets(Room room)
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
        private static void RoomTick(Room room)
        {
            // Not a fan of this, but I cannot come up with a better way of doing it.
            // I'm not going to use a timer because I don't want to have to deal with threading issues.
            // I'm not going to use a stopwatch because I don't want to have to deal with making a bunch more objects per room.
            // This needs to run when UpdateRoomPackets is not running.
            TimeSpan elapsedTime = DateTime.Now - room.m_LastTickTime;
            if (elapsedTime.TotalMilliseconds >= room.m_DelayBetweenTicks)
            {
                room.m_LastTickTime = DateTime.Now;
                TickHandler.Handle(room);
            }
        }
    }
}
