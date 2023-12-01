using TABGCommunityServer.DataTypes;

namespace TABGCommunityServer.Rooms
{
    public partial class Room
    {
        public Dictionary<int, Item> Items { get; private set; } = new Dictionary<int, Item>();
        public int CurrentID = 0;
        public void SpawnItem(Item item)
        {
            Items[item.Id] = item;
            CurrentID++;
        }
        public void RemoveItem(Item item)
        {
            Items.Remove(item.Id);
        }
    }
}
