#define UsingTabgServerList

using ComputerysUltimateTABGServer.Interface;
using ComputerysUltimateTABGServer.Rooms;

namespace ComputerysUltimateTABGServer
{
    class TABGCommunityServer
    {
        static void Main()
        {
            StartUp();
            while (TerminalInterface.ApplicationRunning) { MainLoop(); }
            ShutDown();
        }

        static void StartUp()
        {
            TerminalInterface.printLogo();

            ENet.Library.Initialize();

            for (int i = 0; i < 5; i++) { RoomManager.MakeRoom((ushort)(7777 + i), 50, $"CUTS TEST SERVER {i + 1}", 120); }

#if UsingTabgServerList
            if (RoomManager.ActiveRooms.Count <= 10) { TabgServerList.TabgServerListManager.StartServerListHeartbeat(); }
#endif
        }

        static void MainLoop()
        {
        }

        static void ShutDown()
        {
            RoomManager.EndAllRooms();
        }
    }
}