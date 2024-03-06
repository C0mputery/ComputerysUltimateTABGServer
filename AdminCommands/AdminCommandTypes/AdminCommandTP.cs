using ComputerysUltimateTABGServer.DataTypes.Player;
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
        public static void AdminCommandTP(Peer peer, string[] CommandArguments, Room room)
        {
            if (CommandArguments.Length == 1)
            {
                if (!room.TryToGetPlayer(peer, out Player? Player)) { return; }
                string playerNameOrID = CommandArguments[0];
                if (byte.TryParse(playerNameOrID, out byte playerID))
                {
                    if (room.m_Players.TryGetValue(playerID, out Player? playerToTPtoo))
                    {
                        // This is not done yet!!!
                    }
                }
            }
            return;
        }
    }
}
