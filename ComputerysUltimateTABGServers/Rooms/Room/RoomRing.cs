using ComputerysUltimateTABGServer.DataTypes.Ring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room : IDisposable
    {
        public float m_TimeBeforeFirstRing;
        public float m_BaseRingTime;
        public List<RingLocations> m_RingLocations = [new RingLocations(0,0)];
    }
}
