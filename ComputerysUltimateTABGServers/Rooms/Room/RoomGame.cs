namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room : IDisposable
    {
        public GameMode m_GameMode = GameMode.BattleRoyale; // TO BE ADDED
        public MatchMode m_MatchMode = MatchMode.SQUAD; // TO BE ADDED
        public GameState m_GameState = GameState.WaitingForPlayers; // TO BE ADDED
        public float m_CountDownTimer;
    }
}
