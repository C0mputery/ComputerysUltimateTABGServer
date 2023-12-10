using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.DataTypes.Vehicles
{
    public struct DamageableVehiclePart()
    {
        public byte partIndex = 0;
        public float partHealth = 100;
        public string partName = "";
    }
}
