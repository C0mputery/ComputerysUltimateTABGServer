// We just like hackers now hell yeah

namespace ComputerysUltimateTABGServer.Interface
{
    public static class TerminalInterface
    {
        public static bool ApplicationRunning = true;

        static string CUTSLogo = @"┌──────────────────┐ ┌─────────────────────────────────────────┐
│  ____   _   _   _│ │_   ____      _        ___         ___   │
│ / ___| | | | | |_   _| / ___|    / |      / _ \       / _ \  │
│| |     | | | |   │ │   \___ \    | |     | | | |     | | | | │
│| |___  | |_| |   │ │    ___) |   | |  _  | |_| |  _  | |_| | │
│ \____|  \___/    │ │   |____/    |_| (_)  \___/  (_)  \___/  │
└──────────────────┘ └─────────────────────────────────────────┘";
        public static void printLogo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(CUTSLogo);
            Console.ResetColor();
        }
    }
}
