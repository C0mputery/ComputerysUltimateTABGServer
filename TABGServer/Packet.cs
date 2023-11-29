
namespace TABGCommunityServer
{
    internal struct Packet
    {
        public EventCode Type { get; set; }

        public byte[] Data { get; set; }

        public Packet(EventCode type, byte[] data)
        {
            Data = data;
            Type = type;
        }
    }
}
