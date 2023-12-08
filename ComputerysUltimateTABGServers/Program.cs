using ComputerysUltimateTABGServers.Rooms;

namespace ComputerysUltimateTABGServers
{
    class TABGCommunityServer
    {
        static void Main()
        {
            StartUp();
            while (!Console.KeyAvailable) { MainLoop(); }
            ShutDown();
        }

        static void StartUp()
        {
            Console.WriteLine("TABG COMMUNITY SERVER V2 BOOTING");

            ENet.Library.Initialize();

            RoomManager.MakeRoom(9997, 100);
            RoomManager.StartServerListHeartbeat();
        }

        static void MainLoop()
        {
            RoomManager.UpdateRooms();
        }

        static void ShutDown()
        {
            RoomManager.CloseAllRooms();
        }
    }
}