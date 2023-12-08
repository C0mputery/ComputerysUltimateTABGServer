#define UsingTabgServerList

using ComputerysUltimateTABGServer.Rooms;

namespace ComputerysUltimateTABGServer
{
    class TABGCommunityServer
    {
        static string cutsLogo = @"   ____   _   _   _____   ____      _        ___         ___  
  / ___| | | | | |_   _| / ___|    / |      / _ \       / _ \ 
 | |     | | | |   | |   \___ \    | |     | | | |     | | | |
 | |___  | |_| |   | |    ___) |   | |  _  | |_| |  _  | |_| |
  \____|  \___/    |_|   |____/    |_| (_)  \___/  (_)  \___/ ";

        static void Main()
        {
            StartUp();
            while (!Console.KeyAvailable) { MainLoop(); }
            ShutDown();
        }

        static void StartUp()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(cutsLogo);
            Console.ResetColor();

            ENet.Library.Initialize();

            RoomManager.MakeRoom(7777, 50, "CUTS TEST SERVER 1");
            RoomManager.MakeRoom(7778, 50, "CUTS TEST SERVER 2");
            RoomManager.MakeRoom(7779, 50, "CUTS TEST SERVER 3");


#if UsingTabgServerList
            TabgServerList.TabgServerListManager.StartServerListHeartbeat();
            #endif
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