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
            Console.WriteLine("CUTS BOOTING, LETS FUCKIN GO");

            ENet.Library.Initialize();

            RoomManager.MakeRoom(4949, 100, "CUTS TEST SERVER");
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