using ComputerysUltimateTABGServer.AdminCommands;
using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Interface.Logging;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.Collections.Frozen;

namespace ComputerysUltimateTABGServer.Packets
{
    public static class PacketManager
    {
        public static readonly FrozenDictionary<EventCode, PacketHandlerDelegate[]> PacketHandlers = new Dictionary<EventCode, PacketHandlerDelegate[]>
        {
            { EventCode.RoomInit, new PacketHandlerDelegate[] { PacketTypes.RoomInitPacket } },
            { EventCode.RequestWorldState, new PacketHandlerDelegate[] { PacketTypes.RequestWorldStatePacket } },
            { EventCode.PlayerUpdate, new PacketHandlerDelegate[] { PacketTypes.PlayerUpdatePacket } },
            { EventCode.GearChange,  new PacketHandlerDelegate[]  { PacketTypes.GearChangePacket } },
            { EventCode.TABGPing, new PacketHandlerDelegate[] {  PacketTypes.TabgPingPacket } },
            { EventCode.ChatMessage,  new PacketHandlerDelegate[] { PacketTypes.ChatMessagePacket, AdminCommandManager.HandleAdminCommand} },
            { EventCode.PlayerDead,  new PacketHandlerDelegate[] { PacketTypes.PlayerDeathPacket } },
            { EventCode.SendCatchPhrase, new PacketHandlerDelegate[] { PacketTypes.SendCatchPhrasePacket } },
            { EventCode.ThrowChatMessage, new PacketHandlerDelegate[] { PacketTypes.ThrowChatMessagePacket } }
        }.ToFrozenDictionary();

        public static void PacketHandler(EventCode eventCode, Peer peer, byte[] packetData, Room room)
        {
            if (eventCode is not (EventCode.PlayerUpdate or EventCode.TABGPing))
            {
                room.TryToGetPlayer(peer, out Player? player);
                if (player == null)
                {
                    CUTSLogger.Log($"{room.m_RoomName} | Handling packet: {eventCode}, from a non-player, peer ID: {peer.ID}, peer IP: {peer.IP}", LogLevel.Info);
                }
                else
                {
                    CUTSLogger.Log($"{room.m_RoomName} | Handling packet: {eventCode}, from player: {player.m_Name}, peer ID: {peer.ID}, peer IP: {peer.IP}", LogLevel.Info);
                }
            }

            if (PacketHandlers.TryGetValue(eventCode, out PacketHandlerDelegate[]? packetHandlers))
            {
                foreach (PacketHandlerDelegate packetHandler in packetHandlers)
                {
                    packetHandler(peer, packetData, room);
                }
            }
        }
        
        public static void SendPacketToPlayer(EventCode eventCode, byte[] packetData, Player recipent, Room room)
        {
            SendPacketToPeer(eventCode, packetData, recipent.m_Peer, room);
        }
        public static void SendPacketToPlayers(EventCode eventCode, byte[] packetData, Player[] recipents, Room room)
        {
            SendPacketToPeers(eventCode, packetData, recipents.Select(player => player.m_Peer).ToArray(), room);
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

        public static void SendPacketToPeer(EventCode eventCode, byte[] packetData, Peer recipent, Room room)
        {
            if (eventCode is not (EventCode.PlayerUpdate or EventCode.TABGPing)) { CUTSLogger.Log($"{room.m_RoomName} | Sending packet: {eventCode}, to player: {room.m_Players[(byte)recipent.ID].m_Name}, peer ID: {recipent.ID}, peer IP: {recipent.IP}", LogLevel.Info); }
            byte[] packetByteArray = new byte[packetData.Length + 1];
            packetByteArray[0] = (byte)eventCode;
            Array.Copy(packetData, 0, packetByteArray, 1, packetData.Length);
            Packet packet = default(Packet);
            packet.Create(packetByteArray, PacketFlags.Reliable);
            recipent.Send(0, ref packet);
        }
        public static void SendPacketToPeers(EventCode eventCode, byte[] packetData, Peer[] recipents, Room room)
        {
            if (recipents.Length == 0) { return; }
            if (eventCode is not (EventCode.PlayerUpdate or EventCode.TABGPing)) { CUTSLogger.Log($"{room.m_RoomName} | Sending packet: {eventCode}, to players: {string.Join(", ", recipents.Select(peer => room.m_Players[(byte)peer.ID].m_Name))}, peer IDs: {string.Join(", ", recipents.Select(peer => peer.ID))}, peer IPs: {string.Join(", ", recipents.Select(peer => peer.IP))}", LogLevel.Info); }
            byte[] packetByteArray = new byte[packetData.Length + 1];
            packetByteArray[0] = (byte)eventCode;
            Array.Copy(packetData, 0, packetByteArray, 1, packetData.Length);
            Packet packet = default(Packet);
            packet.Create(packetByteArray, PacketFlags.Reliable);
            Peer[] peersArray = recipents.ToArray();
            room.m_EnetServer.Broadcast(0, ref packet, peersArray);
        }
        public static void SendPacketToAllPeersExcept(EventCode eventCode, byte[] packetData, Peer except, Room room)
        {
            // We do this rather than a normal broadcast so that if theres a peer thats not a player it will not get the packet (idk if this can happen just being safe)
            SendPacketToPeers(eventCode, packetData, room.m_Players.Values.Select(player => player.m_Peer).Where(player => !player.Equals(except)).ToArray(), room);
        }
        public static void SendPacketToAllPeersExcept(EventCode eventCode, byte[] packetData, Peer[] except, Room room)
        {
            // We do this rather than a normal broadcast so that if theres a peer thats not a player it will not get the packet (idk if this can happen just being safe)
            SendPacketToPeers(eventCode, packetData, room.m_Players.Values.Select(player => player.m_Peer).Except(except).ToArray(), room);
        }
    }

    public delegate void PacketHandlerDelegate(Peer peer, byte[] packetDataRaw, Room room);
}