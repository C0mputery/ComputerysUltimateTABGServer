using ComputerysUltimateTABGServer.DataTypes.Items;
using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Packets;

namespace ComputerysUltimateTABGServer.Rooms
{
    public partial class Room
    {
        public List<Item> m_Items = [];

        public void GivePlayerLoot(Item item, Player player)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write((ushort)1); // How many items we are giving.
                binaryWriter.Write(item.WeaponIndex);
                binaryWriter.Write(item.Quantity);
                binaryWriter.Write(0); // Not sure what this is as it is discarded by the client.
                PacketManager.SendPacketToPlayer(EventCode.PlayerLootRecieved, memoryStream.ToArray(), player, this);

            }
        }
        public void GivePlayerLoot(Item[] items, Player player)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write((ushort)items.Length);
                foreach (Item item in items)
                {
                    binaryWriter.Write(item.WeaponIndex);
                    binaryWriter.Write(item.Quantity);
                }
                binaryWriter.Write(0); // Not sure what this is as it is discarded by the client.
                PacketManager.SendPacketToPlayer(EventCode.PlayerLootRecieved, memoryStream.ToArray(), player, this);
            }
        }
    }
}