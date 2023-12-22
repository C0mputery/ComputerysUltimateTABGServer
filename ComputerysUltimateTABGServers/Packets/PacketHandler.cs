using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Interface.Logging;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.Collections.Frozen;

namespace ComputerysUltimateTABGServer.Packets
{
    public static class PacketHandler
    {
        public static readonly FrozenDictionary<EventCode, PacketHandlerDelegate> PacketHandlers = new Dictionary<EventCode, PacketHandlerDelegate>
        {
            { EventCode.RoomInit, PacketTypes.RoomInitPacket },
            { EventCode.RequestWorldState, PacketTypes.RequestWorldStatePacket },
            { EventCode.PlayerUpdate, PacketTypes.PlayerUpdatePacket },
        }.ToFrozenDictionary();
        public static void Handle(EventCode eventCode, Peer peer, byte[] packetData, Room room)
        {
            if ((eventCode != EventCode.TABGPing) && (eventCode != EventCode.PlayerUpdate))
            {
                room.TryToGetPlayer(peer, out Player? player);
                if (player == null)
                {
                    CUTSLogger.Log($"{room.m_RoomName} | Handling packet: {eventCode}, from a non-player, peer ID: {peer.ID}, peer IP: {peer.IP}", LogLevel.Info);
                }
                else
                {
                    CUTSLogger.Log($"{room.m_RoomName} | Handling packet: {eventCode}, from player: {player.m_Name}", LogLevel.Info);
                }
            }

            using (MemoryStream packetDataMemoryStream = new MemoryStream(packetData))
            using (BinaryReader packetDataBinaryReader = new BinaryReader(packetDataMemoryStream))
            {
                if (PacketHandlers.TryGetValue(eventCode, out PacketHandlerDelegate? packetHandler))
                {
                    packetHandler(peer, packetDataBinaryReader, room);
                }
            }
        }

        public static void SendPacketToPlayer(EventCode eventCode, byte[] packetData, Player recipent, Room room)
        {
            if (eventCode != EventCode.PlayerUpdate) { CUTSLogger.Log($"{room.m_RoomName} | Sending packet: {eventCode}, to: {recipent.m_Name}", LogLevel.Info); }
            byte[] packetByteArray = new byte[packetData.Length + 1];
            packetByteArray[0] = (byte)eventCode;
            Array.Copy(packetData, 0, packetByteArray, 1, packetData.Length);
            Packet packet = default(Packet);
            packet.Create(packetByteArray, PacketFlags.Reliable);
            recipent.m_Peer.Send(0, ref packet);
        }
        public static void SendPacketToPlayers(EventCode eventCode, byte[] packetData, Player[] recipents, Room room)
        {
            if (eventCode != EventCode.PlayerUpdate) { CUTSLogger.Log($"{room.m_RoomName} | Sending packet: {eventCode}, to: {string.Join(", ", recipents.Select(player => player.m_Name))}", LogLevel.Info); }
            byte[] packetByteArray = new byte[packetData.Length + 1];
            packetByteArray[0] = (byte)eventCode;
            Array.Copy(packetData, 0, packetByteArray, 1, packetData.Length);
            Packet packet = default(Packet);
            packet.Create(packetByteArray, PacketFlags.Reliable);
            Peer[] peersArray = recipents.Select(player => player.m_Peer).ToArray();
            room.m_EnetServer.Broadcast(0, ref packet, peersArray);
        }
        public static void SendPacketToAllPlayers(EventCode eventCode, byte[] packetData, Room room)
        {
            // We do this rather than a normal broadcast so that if theres a peer thats not a player it will not get the packet (idk if this can happen just being safe)
            SendPacketToPlayers(eventCode, packetData, room.m_Players.Values.ToArray(), room);
        }
        public static void SendPacketToAllPlayersExcept(EventCode eventCode, byte[] packetData, Player except, Room room)
        {
            // We do this rather than a normal broadcast so that if theres a peer thats not a player it will not get the packet (idk if this can happen just being safe)
            SendPacketToPlayers(eventCode, packetData, room.m_Players.Values.Where(player => player != except).ToArray(), room);
        }
        public static void SendPacketToAllPlayersExcept(EventCode eventCode, byte[] packetData, Player[] except, Room room)
        {
            // We do this rather than a normal broadcast so that if theres a peer thats not a player it will not get the packet (idk if this can happen just being safe)
            SendPacketToPlayers(eventCode, packetData, room.m_Players.Values.Except(except).ToArray(), room);
        }
    }

    public delegate void PacketHandlerDelegate(Peer peer, BinaryReader packetDataBinaryReader, Room room);
}