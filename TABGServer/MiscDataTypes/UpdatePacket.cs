namespace TABGCommunityServer.MiscDataTypes
{
    public struct UpdatePacket(byte[] packet, Player broadcastPlayer)
    {
        public byte[] Packet { get; set; } = packet;
        public Player BroadcastPackets { get; set; } = broadcastPlayer;
    }
}
