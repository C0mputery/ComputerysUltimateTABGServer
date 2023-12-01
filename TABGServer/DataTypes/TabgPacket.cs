namespace TABGCommunityServer.DataTypes
{
    public struct TabgPacket
    {
        public EventCode Type { get; set; }

        public byte[] Data { get; set; }

        public TabgPacket(EventCode type, byte[] data)
        {
            Data = data;
            Type = type;
        }
    }
}
