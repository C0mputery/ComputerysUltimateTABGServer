using ComputerysUltimateTABGServer.Rooms;
using System.Collections.Frozen;

namespace ComputerysUltimateTABGServer.Ticks
{
    static public class TickManager
    {
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
