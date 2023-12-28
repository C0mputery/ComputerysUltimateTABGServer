using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.DataTypes.Ring
{
    public struct RingLocations(float ringSize, float ringSpeed)
    {
        public float ringSize = ringSize;
        public float ringSpeed = ringSpeed;
    }
}
