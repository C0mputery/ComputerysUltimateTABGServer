namespace TABGCommunityServer.MiscDataTypes
{
    public struct TabgPacket(EventCode type, byte[] data)
    {
        public EventCode Type { get; set; } = type;

        public byte[] Data { get; set; } = data;
    }
}
