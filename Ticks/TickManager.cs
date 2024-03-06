using ComputerysUltimateTABGServer.Rooms;
using System.Collections.Frozen;

namespace ComputerysUltimateTABGServer.Ticks
{
    static public class TickManager
    {
        // This apllies the same principle as the other delegates in the project. It's like this so it can be easily expanded upon at runtime.
        public static readonly FrozenSet<TickHandlerDelegate> TickHandlers = new HashSet<TickHandlerDelegate>
        {
            TickTypes.UpdateTick,
        }.ToFrozenSet();

        public static void Handle(Room room)
        {
            foreach (TickHandlerDelegate tickHandler in TickHandlers) { tickHandler(room); }
        }
    }

    public delegate void TickHandlerDelegate(Room room);
}
