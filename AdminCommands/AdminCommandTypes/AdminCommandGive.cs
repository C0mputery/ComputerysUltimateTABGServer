using ComputerysUltimateTABGServer.DataTypes.Items;
using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.AdminCommands.AdminCommandTypes
{
    public static partial class AdminCommandTypes
    {
        public static void AdminCommandGive(Peer peer, string[] CommandParts, Room room)
        {
            if (CommandParts.Length == 2)
            {
                if (!room.TryToGetPlayer(peer, out Player? player)) { return; }
                if (!int.TryParse(CommandParts[0], out int weaponIndex) || !int.TryParse(CommandParts[1], out int quantity))
                {
                    return;
                }
                room.GivePlayerLoot(new Item() { WeaponIndex = 1, Quantity = 1}, player);
            }
        }
    }
}
