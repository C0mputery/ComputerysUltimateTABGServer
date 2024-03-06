// Comment this out if you don't want to post to the server list.
#define UsingServerList

using ComputerysUltimateTABGServer.Interface;
using ComputerysUltimateTABGServer.Rooms;

namespace ComputerysUltimateTABGServer
{
    class ComputerysUltimateTABGServer
    {
        static void Main()
        {
            // This stuff is broken out into functions for readability.
            // If theres a better way to do this, please tell me.
            StartUp();
            while (!Console.KeyAvailable) { MainLoop(); } // This while loop will be replaced with a loop that reads from the terminal interface.
            ShutDown();
        }

        static void StartUp()
        {
            // StartUp calls for plugins and other things that need to be initialized will go here.

            ENet.Library.Initialize(); // We need to initialize ENet before we can use it, duh.

            TerminalInterface.printLogo(); // Prints cool ascii art when the init is done.

            // This loop is temporary, it will be replaced with a loop that reads from a config file.
            // This is creating rooms for testing purposes.
            for (int i = 0; i < 1; i++) { RoomManager.MakeRoom((ushort)(7777 + i), 50, "testing", 120); }

#if UsingServerList
            // This is the server list code, it should be commented out if you don't have access to it.
            // Includes a simple check to make sure that the server list doesn't get spammed with servers by accident.
            if (RoomManager.ActiveRooms.Count <= 10) { TabgServerList.ServerListManager.StartServerListHeartbeat(); }
#endif
        }

        static void MainLoop()
        {
            // This will have the terminal interface logic sometime in the future.
            // ui running on the main thread, and the servers running on a different threads.
        }

        static void ShutDown()
        {
            RoomManager.EndAllRooms(); // This will end all rooms
            ENet.Library.Deinitialize(); // I don't know if this is needed, but I am doing it anyway.
        }
    }
}

    