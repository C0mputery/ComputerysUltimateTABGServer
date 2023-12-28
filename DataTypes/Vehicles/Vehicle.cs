using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.DataTypes.Vehicles
{
    public struct Vehicle()
    {
        public int vehicleID = 0;
        public int vehicleIndex = 0;
        public List<Seat> seats = [];
        public List<DamageableVehiclePart> damageableVehicleParts = [];
        public Vector3 position = new Vector3(0, 0, 0);
        public Vector3 carInput = new Vector3(0, 0, 0);
        public Quaternion rotation = new Quaternion(0, 0, 0, 0);
    }
}
