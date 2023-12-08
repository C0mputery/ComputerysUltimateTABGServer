namespace ComputerysUltimateTABGServers.MiscDataTypes
{
    public class Group(bool shouldAutoFill, ulong groupLoginKey, byte groupIndex)
    {
        public List<byte> m_PlayerIDs = new List<byte>();

        public int m_MaxPlayers = 4;

        public bool m_ShouldAutoFill = shouldAutoFill;

        public ulong m_GroupLoginKey = groupLoginKey;

        public byte m_GroupIndex = groupIndex;
    }
}
