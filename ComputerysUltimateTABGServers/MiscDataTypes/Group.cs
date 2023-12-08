namespace ComputerysUltimateTABGServers.MiscDataTypes
{
    public class Group(bool shouldAutoFill, ulong groupLoginKey, byte groupIndex)
    {
        public List<Player> m_Players = new List<Player>();

        public int m_MaxPlayers = 4;

        public bool m_ShouldAutoFill = shouldAutoFill;

        public ulong m_GroupLoginKey = groupLoginKey;

        public byte m_GroupIndex = groupIndex;

        bool IsOpen()
        {
            return (m_Players.Count < m_MaxPlayers);
        }
    }
}
