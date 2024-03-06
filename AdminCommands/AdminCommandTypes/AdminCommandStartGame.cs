using ComputerysUltimateTABGServer.DataTypes.Items;
using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Interface.Logging;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.AdminCommands
{
    public static partial class AdminCommandTypes
    {
        public static void AdminCommandStartGame(Peer peer, string[] CommandArguments, Room room)
        {
            if (CommandArguments.Length == 1)
            {
                if (float.TryParse(CommandArguments[0], out float time))
                {
                    room.startCountDownTimer(time);
                }
            }
        }
    }
}
