using ComputerysUltimateTABGServer.DataTypes.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room
    {
        public Dictionary<int, Vehicle> m_Vehicles = new Dictionary<int, Vehicle>();
    }
}
