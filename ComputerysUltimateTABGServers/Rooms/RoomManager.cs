using ComputerysUltimateTABGServer.Packets;
using ENet;
using System.Text;
using System.Text.Json;

namespace ComputerysUltimateTABGServer.Rooms
{
    public static class RoomManager
    {
        public static List<Room> Rooms { get; private set; } = [];

        public static void MakeRoom(ushort port, int maxPlayers, string roomName)
        {
            Room room = new(port, maxPlayers, roomName);
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
                room.m_EnetEvent.Packet.Dispose();
            }
        }

        public static void CloseRoom(Room room)
        {
            room.m_EnetServer.Flush();
            Rooms.Remove(room);
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

                    break;
            }
        }
    }
}
