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
        public static void AdminCommandStartGame(Peer peer, string[] CommandParts, Room room)
        {
            if (CommandParts.Length == 1)
            {
                if (float.TryParse(CommandParts[0], out float time))
                {
                    room.startCountDownTimer(time);
                }
            }
        }
    }
}
