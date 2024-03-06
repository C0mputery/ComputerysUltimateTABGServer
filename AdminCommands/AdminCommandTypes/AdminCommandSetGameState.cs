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
        public static void AdminCommandSetGameState(Peer peer, string[] CommandArguments, Room room)
        {
            if (CommandArguments.Length == 1)
            {
                GameState gameState = (GameState)Enum.Parse(typeof(GameState), CommandArguments[0], true);
                room.setGameState(gameState, new byte[0]);
            }
        }
    }
}
