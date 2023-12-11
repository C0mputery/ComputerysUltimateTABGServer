using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.Interface.Logging
{
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
        Fatal = 4,
    }

    public static partial class CUTSLogger
    {
        public static void Log(string message, LogLevel loggingLevel)
        {
            Console.WriteLine(message);
        }
    }
}
