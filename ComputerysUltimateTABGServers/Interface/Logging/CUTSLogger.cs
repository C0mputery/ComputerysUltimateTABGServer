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
        // log colors in there current state are not thread safe, and sometimes will be printed in the wrong color. If you can fix this, please do!
        public static void Log(string message, LogLevel loggingLevel)
        {
            Console.ForegroundColor = (ConsoleColor)loggingLevel;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
