using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABGCommunityServer.DataTypes;

namespace TABGCommunityServer
{
    public class Player
    {
        public byte Id { get; set; }
        public byte Group { get; set; }
        public string Name { get; set; }
        public (float X, float Y, float Z) Location { get; set; }
        public (float X, float Y) Rotation { get; set; }
        public int[] GearData { get; set; }
        public float Health { get; set; }
        public bool Ads { get; set; } // Aim down sights
        public byte[] OptimizedDirection { get; set; }
        public List<TabgPacket> PendingBroadcastPackets { get; set; }
        public byte MovementFlags { get; set; }

        public Player(byte id, byte group, string name, (float X, float Y, float Z) location, (float X, float Y) rotation, int[] gearData)
        {
            Id = id;
            Group = group;
            Name = name;
            Rotation = rotation;
            Location = location;
            Ads = false;
            GearData = gearData;
            OptimizedDirection = new byte[3];
            MovementFlags = 0;
            PendingBroadcastPackets = new List<TabgPacket>();
            Health = 100f;
        }
    }
}
