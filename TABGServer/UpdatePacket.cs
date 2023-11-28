namespace TABGCommunityServer
{
    internal class UpdatePacket
    {
        public byte[] Packet { get; set; }
        public Player BroadcastPackets { get; set; }

        public UpdatePacket(byte[] packet, Player broadcastPlayer)
        {
            Packet = packet;
            BroadcastPackets = broadcastPlayer;
        }
    }
}
