using ENet;
using ComputerysUltimateTABGServer.Rooms;
using ComputerysUltimateTABGServer.MiscDataTypes;

namespace ComputerysUltimateTABGServer.Packets.PacketTypes
{
    public struct RequestWorldStatePacket : IPacket
    {
        public void Handle(Peer peer, BinaryReader receivedPacketData, Room room)
        {
            if (!room.TryToGetPlayer(receivedPacketData.ReadByte(), out Player? player))
            {
                return;
            }

        }

    }
}