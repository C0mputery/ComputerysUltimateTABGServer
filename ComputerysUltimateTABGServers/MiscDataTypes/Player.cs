using ENet;

namespace TABGCommunityServer.MiscDataTypes
{
    public class Player(Peer peer, byte group, string name, (float X, float Y, float Z) location, (float X, float Y) rotation, int[] gearData, bool autoTeam)
    {
        public Peer m_Peer { get; set; } = peer;
        public byte m_GroupID { get; set; } = group;
        public string m_Name { get; set; } = name;
        public (float X, float Y, float Z) m_Location { get; set; } = location;
        public (float X, float Y) m_Rotation { get; set; } = rotation;
        public int[] m_GearData { get; set; } = gearData;
        public float m_Health { get; set; } = 100f;
        public bool m_AimDownSights { get; set; } = false;
        public byte[] m_OptimizedDirection { get; set; } = new byte[3];
        public byte m_MovementFlags { get; set; } = 0;

        public bool m_AutoTeam = autoTeam;
    }
}
