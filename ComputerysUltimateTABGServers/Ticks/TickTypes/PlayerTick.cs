using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using System.Collections.Frozen;
using System.Numerics;

namespace ComputerysUltimateTABGServer.Ticks
{
    public static partial class TickTypes
    {
        public static void PlayerTick(Room room)
        {
            foreach (Player player in room.m_Players.Values)
            {
                Player[] playersInRange = FindPlayersInRange(player, room);
            }
        }

        public static Player[] FindPlayersInRange(Player player, Room room)
        {
            return room.m_Players.Values.Where(playerToCheckDistance => Vector3.Distance(playerToCheckDistance.m_Position, player.m_Position) <= room.m_PlayerUpdateRange).ToArray();
        }
    }
}
