using ENet;
using System.Text;

namespace TABGCommunityServer.Packets
{
    public class RoomInitPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            PacketHandler.BroadcastPacket(peer, EventCode.PlayerDead, new PlayerHandler().SendNotification(0, "WELCOME - RUNNING COMMUNITY SERVER V2.TEST"), true);
        }
    }

    public class ChatMessagePacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            var playerIndex = binaryReader.ReadByte(); // or ReadInt32(), depending on the type of PlayerIndex
            var messageLength = binaryReader.ReadByte();
            var messageBytes = binaryReader.ReadBytes(messageLength);
            var message = Encoding.Unicode.GetString(messageBytes);
            Console.WriteLine("Player " + playerIndex + ": " + message);

            var handler = new AdminCommandHandler(message, playerIndex);
            handler.Handle();

            // test if there needs to be a packet sent back
            if (handler.shouldSendPacket)
            {
                PacketHandler.BroadcastPacket(peer, handler.code, handler.packetData, true);
            }

            if (handler.notification != null && handler.notification != "")
            {
                PacketHandler.BroadcastPacket(peer, EventCode.PlayerDead, new PlayerHandler().SendNotification(playerIndex, handler.notification), true);
            }
        }
    }

    public class RequestItemThrowPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            PacketHandler.BroadcastPacket(peer, EventCode.ItemThrown, Throwables.ClientRequestThrow(binaryReader), true);
        }
    }

    public class RequestItemDropPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            PacketHandler.BroadcastPacket(peer, EventCode.ItemDrop, Droppables.ClientRequestDrop(binaryReader), true);
        }
    }

    public class RequestWeaponPickUpPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            PacketHandler.BroadcastPacket(peer, EventCode.WeaponPickUpAccepted, Droppables.ClientRequestPickUp(binaryReader), true);
        }
    }

    public class PlayerUpdatePacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            // this packet is different because it can have an unlimited amount of subpackets
            UpdatePacket updatePacket = new PlayerHandler().PlayerUpdate(binaryReader, buffer.Length);

            PacketHandler.BroadcastPacket(peer, EventCode.PlayerUpdate, updatePacket.Packet, true);

            // have to do this so enumeration is safe
            List<Packet> packetList = updatePacket.BroadcastPackets.PendingBroadcastPackets;

            // also use this packet to send pending broadcast packets
            foreach (var item in packetList)
            {
                PacketHandler.BroadcastPacket(peer, item.Type, item.Data, true);
            }

            updatePacket.BroadcastPackets.PendingBroadcastPackets = new List<Packet>();
        }
    }

    public class WeaponChangePacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            PacketHandler.BroadcastPacket(peer, EventCode.WeaponChanged, new PlayerHandler().PlayerChangedWeapon(binaryReader), true);
        }
    }

    public class PlayerFirePacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            new PlayerHandler().PlayerFire(binaryReader, buffer.Length);
        }
    }

    public class RequestSyncProjectileEventPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            PacketHandler.BroadcastPacket(peer, EventCode.SyncProjectileEvent, new PlayerHandler().ClientRequestProjectileSyncEvent(binaryReader, buffer.Length), true);
        }
    }

    public class RequestAirplaneDropPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            PacketHandler.BroadcastPacket(peer, EventCode.PlayerAirplaneDropped, new PlayerHandler().RequestAirplaneDrop(binaryReader), true);
        }
    }

    public class DamageEventPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            new PlayerHandler().PlayerDamagedEvent(binaryReader);
        }
    }

    public class RequestBlessingPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            PacketHandler.BroadcastPacket(peer, EventCode.BlessingRecieved, new PlayerHandler().RequestBlessingEvent(binaryReader), true);
        }
    }

    public class RequestHealthStatePacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader)
        {
            PacketHandler.BroadcastPacket(peer, EventCode.PlayerHealthStateChanged, new PlayerHandler().RequestHealthState(binaryReader), true);
        }
    }
}
