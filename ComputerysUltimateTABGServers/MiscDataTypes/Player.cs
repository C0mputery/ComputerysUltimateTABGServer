using ENet;

namespace ComputerysUltimateTABGServer.MiscDataTypes
{
    public class Player(Peer peer, string name, byte groupID, int[] gearData, string playFabID, int color)
    {
        public Peer m_Peer { get; set; } = peer;
        public byte m_GroupIndex { get; set; } = groupID;
        public string m_Name { get; set; } = name;
        public (float X, float Y, float Z) m_Location { get; set; }
        public (float X, float Y) m_Rotation { get; set; }
        public int[] m_GearData { get; set; } = gearData;
        public float m_Health { get; set; } = 100f;
        public bool m_AimDownSights { get; set; } = false;
        public byte[] m_OptimizedDirection { get; set; } = new byte[3];
        public byte m_MovementFlags { get; set; } = 0;

        public string m_PlayFabID = playFabID;

        public int color = color;
    }
}
