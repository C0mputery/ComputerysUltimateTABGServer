using ComputerysUltimateTABGServer.DataTypes.Ring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room
    {
        public float timeBeforeFirstRing;
        public float baseRingTime;
        public List<RingLocations> ringLocations = new();
    }
}
