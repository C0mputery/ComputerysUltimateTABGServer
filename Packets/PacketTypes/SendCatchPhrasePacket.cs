using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Interface.Logging;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.Reflection;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void SendCatchPhrasePacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {
                if (!room.TryToGetPlayer(peer, out Player? player)) { return; }
                byte[] packet = { player.m_PlayerID };
                PacketManager.SendPacketToAllPlayers(EventCode.CatchPhrase, packet, room);
        }
    }
}