using ENet;
using System.Numerics;

namespace ComputerysUltimateTABGServer.DataTypes.Player
{
    public class Player(Peer peer, string name, byte groupID, int[] gearData, string playFabID, int color)
    {
        public Peer m_Peer = peer;
        public byte m_PlayerID = (byte)peer.ID;
        public byte m_GroupIndex = groupID;
        public string m_Name = name;
        public int[] m_GearData = gearData;
        public string m_PlayFabID = playFabID;
        public int m_Color = color;
        public bool m_IsDev = false;

        public Vector3 m_Position = new Vector3(0, 0, 0);
        public float m_Rotation = 0;

        public bool m_IsDead = false;
        public bool m_IsDowned = false;
        public float m_Health = 100;

        public bool m_IsInCar = false; // TO BE ADDED

        public int m_CurrentlyHeldWeaponID = int.MaxValue; // TO BE ADDED

        public int m_OccupiedCarId = 0;
        public int m_OccupiedSeatId = 0;
    }
}
