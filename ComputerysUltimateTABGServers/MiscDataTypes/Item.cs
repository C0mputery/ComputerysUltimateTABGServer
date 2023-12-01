namespace TABGCommunityServer.MiscDataTypes
{
    public struct Item(int id, int localIndex, int count, (float x, float y, float z) loc)
    {
        public int Id { get; set; } = id;
        public int Type { get; set; } = localIndex;
        public int Count { get; set; } = count;
        public (float X, float Y, float Z) Location { get; set; } = loc;
    }
}
