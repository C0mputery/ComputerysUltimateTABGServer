using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.DataTypes.Items
{
    public struct Item()
    {
        public int WeaponIndex = int.MaxValue;
        public int UniqueIdentifier = 0;
        public int Quantity = 0;
        public Vector3 Position = new(0,0,0);
    }
}
