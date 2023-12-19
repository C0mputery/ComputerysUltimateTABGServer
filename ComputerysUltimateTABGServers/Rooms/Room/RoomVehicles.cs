using ComputerysUltimateTABGServer.DataTypes.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room : IDisposable
    {
        public List<Vehicle> m_Vehicles = new List<Vehicle>();
    }
}
