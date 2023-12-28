namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room
    {
        public GameMode m_GameMode = GameMode.BattleRoyale;
        public MatchMode m_MatchMode = MatchMode.SQUAD;
        public GameState m_GameState = GameState.WaitingForPlayers;
        public float m_CountDownTimer;
    }
}
