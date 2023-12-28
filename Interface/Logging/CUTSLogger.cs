using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.Interface.Logging
{
    public enum LogLevel
    {
        Debug = ConsoleColor.Gray,
        Info = ConsoleColor.Green,
        Warning = ConsoleColor.Yellow,
        Error = ConsoleColor.Red,
        Fatal = ConsoleColor.DarkRed,
        Logo = ConsoleColor.Blue
    }

    public static partial class CUTSLogger
    {
        private static readonly object lockObject = new object();

        public static void Log(string message, LogLevel loggingLevel)
        {
            lock (lockObject)
            {
                Console.ForegroundColor = (ConsoleColor)loggingLevel;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
