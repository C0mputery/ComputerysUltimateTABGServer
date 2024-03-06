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
        public static void AdminCommandGive(Peer peer, string[] CommandArguments, Room room)
        {
            if (CommandArguments.Length == 2)
            {
                if (!room.TryToGetPlayer(peer, out Player? player)) { return; }
                if (!int.TryParse(CommandArguments[0], out int weaponIndex) || !int.TryParse(CommandArguments[1], out int quantity))
                {
                    return;
                }
                room.GivePlayerLoot(new Item() { WeaponIndex = weaponIndex, Quantity = weaponIndex }, player);
            }
        }
    }
}
