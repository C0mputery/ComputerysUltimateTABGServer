using TABGCommunityServer.Rooms;

namespace TABGCommunityServer
{
    class TABGCommunityServer
    {
        static void Main()
        {
            Console.WriteLine("TABG COMMUNITY SERVER V2 BOOTING");

            ENet.Library.Initialize();

            RoomManager.MakeRoom(9997, 100);

            while (!Console.KeyAvailable) { MainLoop(); }

            RoomManager.CloseAllRooms();
        }

        static void MainLoop()
        {
            RoomManager.UpdateRooms();
        }
    }
}