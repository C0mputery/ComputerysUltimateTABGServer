using ENet;
using System.Numerics;

namespace ComputerysUltimateTABGServer.MiscDataTypes
{
    public class Player(Peer peer, string name, byte groupID, int[] gearData, string playFabID, int color)
    {
        public Peer m_Peer = peer;
        public byte m_GroupIndex = groupID;
        public string m_Name = name;
        public int[] m_GearData = gearData;
        public string m_PlayFabID = playFabID;
        public int color = color;
        public Vector3 m_Position = new Vector3(0,0,0);
        public float m_Rotation = 0;
    }
}
