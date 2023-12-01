using ENet;
using TABGCommunityServer.Packets;
using TABGCommunityServer.Rooms;
using TABGCommunityServer.ServerData;

namespace TABGCommunityServer
{
    class TABGCommunityServer
    {
        static void Main()
        {
            Console.WriteLine("TABG COMMUNITY SERVER V2 BOOTING");

            ENet.Library.Initialize();

            RoomManager.MakeRoom();

            while (!Console.KeyAvailable) { MainLoop(); }

            RoomManager.CloseAllRooms();
        }

        static void MainLoop()
        {
            RoomManager.UpdateRooms();
        }
    }
}