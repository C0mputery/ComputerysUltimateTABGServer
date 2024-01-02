using ComputerysUltimateTABGServer.DataTypes.Items;
using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.AdminCommands
{
    public static partial class AdminCommandTypes
    {
        public static void AdminCommandTestKit(Peer peer, string[] CommandParts, Room room)
        {

            if (!room.TryToGetPlayer(peer, out Player? player)) { return; }
            Item[] items =
            {
                new Item() { WeaponIndex = 0, Quantity = 10000 },
                new Item() { WeaponIndex = 1, Quantity = 10000 },
                new Item() { WeaponIndex = 2, Quantity = 10000 },
                new Item() { WeaponIndex = 3, Quantity = 10000 },
                new Item() { WeaponIndex = 4, Quantity = 10000 },
                new Item() { WeaponIndex = 6, Quantity = 10000 },
                new Item() { WeaponIndex = 7, Quantity = 10000 },
                new Item() { WeaponIndex = 8, Quantity = 10000 },
                new Item() { WeaponIndex = 9, Quantity = 10000 },
                new Item() { WeaponIndex = 10, Quantity = 10000 },
                new Item() { WeaponIndex = 11, Quantity = 10000 },
                new Item() { WeaponIndex = 29, Quantity = 3 },
                new Item() { WeaponIndex = 32, Quantity = 3 },
                new Item() { WeaponIndex = 36, Quantity = 3 },
                new Item() { WeaponIndex = 75, Quantity = 3 },
                new Item() { WeaponIndex = 151, Quantity = 1 },
                new Item() { WeaponIndex = 202, Quantity = 10000 },
                new Item() { WeaponIndex = 200, Quantity = 10000 },
                new Item() { WeaponIndex = 226, Quantity = 1 },
                new Item() { WeaponIndex = 252, Quantity = 1 },
            };
            room.GivePlayerLoot(items, player);
        }
    }
}
