#define UsingTabgServerList

using ComputerysUltimateTABGServer.Rooms;

namespace ComputerysUltimateTABGServer
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
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"┌──────────────────┐ ┌─────────────────────────────────────────┐
│  ____   _   _   _│ │_   ____      _        ___         ___   │
│ / ___| | | | | |_   _| / ___|    / |      / _ \       / _ \  │
│| |     | | | |   │ │   \___ \    | |     | | | |     | | | | │
│| |___  | |_| |   │ │    ___) |   | |  _  | |_| |  _  | |_| | │
│ \____|  \___/    │ │   |____/    |_| (_)  \___/  (_)  \___/  │
└──────────────────┘ └─────────────────────────────────────────┘");
            Console.ResetColor();

            ENet.Library.Initialize();

            for (int i = 0; i > 10; i++)
            {
                RoomManager.MakeRoom((ushort)(7777 + i), 50, $"CUTS TEST SERVER {i}");
            }


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