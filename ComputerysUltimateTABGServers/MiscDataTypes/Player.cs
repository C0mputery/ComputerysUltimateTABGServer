namespace TABGCommunityServer.MiscDataTypes
{
    public class Player(byte id, byte group, string name, (float X, float Y, float Z) location, (float X, float Y) rotation, int[] gearData)
    {
        public byte Id { get; set; } = id;
        public byte Group { get; set; } = group;
        public string Name { get; set; } = name;
        public (float X, float Y, float Z) Location { get; set; } = location;
        public (float X, float Y) Rotation { get; set; } = rotation;
        public int[] GearData { get; set; } = gearData;
        public float Health { get; set; } = 100f;
        public bool Ads { get; set; } = false;
        public byte[] OptimizedDirection { get; set; } = new byte[3];
        public List<TabgPacket> PendingBroadcastPackets { get; set; } = [];
        public byte MovementFlags { get; set; } = 0;
    }
}
