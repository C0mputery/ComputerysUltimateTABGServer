using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Packets;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room
    {
        public GameMode m_GameMode = GameMode.BattleRoyale;
        public MatchMode m_MatchMode = MatchMode.SQUAD;
        public GameState m_GameState = GameState.WaitingForPlayers;
        public float m_CountDownTimer;

        public void startCountDownTimer(float timeValue)
        {
            m_CountDownTimer = timeValue;
            byte[] timeValueBytes = BitConverter.GetBytes(timeValue);
            byte[] packetData = [(byte)m_GameState, timeValueBytes[0], timeValueBytes[1], timeValueBytes[2], timeValueBytes[3]];
            setGameState(packetData);
        }

        // These can be moved to another place and can become static.
        public void setGameState(GameState gameState)
        {
            byte[] packetData = new byte[] { (byte)gameState };
            setGameState(packetData);
        }
        public void setGameState(GameState gameState, byte[] additionalData)
        {
            byte[] packetData = new byte[additionalData.Length + 1];
            packetData[0] = (byte)gameState;
            Array.Copy(additionalData, 0, packetData, 1, additionalData.Length);
            setGameState(packetData);
        }
        public void setGameState(byte[] packetData)
        {
            PacketManager.SendPacketToAllPlayers(EventCode.GameStateChanged, packetData, this);
        }
    }
}