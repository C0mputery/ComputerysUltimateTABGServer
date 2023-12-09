//#define UsingTabgServerList

using ComputerysUltimateTABGServer.Rooms;

namespace ComputerysUltimateTABGServer
{
    class TABGCommunityServer
    {
        static string cutsLogo = @"┌──────────────────┐ ┌─────────────────────────────────────────┐
│  ____   _   _   _│ │_   ____      _        ___         ___   │
│ / ___| | | | | |_   _| / ___|    / |      / _ \       / _ \  │
│| |     | | | |   │ │   \___ \    | |     | | | |     | | | | │
│| |___  | |_| |   │ │    ___) |   | |  _  | |_| |  _  | |_| | │
│ \____|  \___/    │ │   |____/    |_| (_)  \___/  (_)  \___/  │
└──────────────────┘ └─────────────────────────────────────────┘";

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

            RoomManager.MakeRoom(7777, 50, "CUTS TEST SERVER");
            /*RoomManager.MakeRoom(7777+1, 50, "CUTS TEST SERVER");
            RoomManager.MakeRoom(7777+2, 50, "CUTS TEST SERVER");
            RoomManager.MakeRoom(7777+3, 50, "CUTS TEST SERVER");
            RoomManager.MakeRoom(7777+4, 50, "CUTS TEST SERVER");
            RoomManager.MakeRoom(7777+5, 50, "CUTS TEST SERVER");
            RoomManager.MakeRoom(7777+6, 50, "CUTS TEST SERVER");
            RoomManager.MakeRoom(7777+7, 50, "CUTS TEST SERVER");
            RoomManager.MakeRoom(7777+8, 50, "CUTS TEST SERVER");
            RoomManager.MakeRoom(7777+9, 50, "CUTS TEST SERVER");*/


#if UsingTabgServerList
            TabgServerList.TabgServerListManager.StartServerListHeartbeat();
#endif
        }
        static int i = 0;
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