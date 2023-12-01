using ENet;
using System.Text;
using TABGCommunityServer.MiscDataTypes;
using TABGCommunityServer.Rooms;

namespace TABGCommunityServer.Packets
{
    public class RoomInitPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            string roomName = "DecompileServer";
            byte newIndex = room.LastID++;

            string playerName = binaryReader.ReadString();
            string gravestoneText = binaryReader.ReadString();
            ulong loginKey = binaryReader.ReadUInt64();
            bool squadHost = binaryReader.ReadBoolean();
            byte squadMembers = binaryReader.ReadByte();
            int userGearLength = binaryReader.ReadInt32();

            int[] gearData = new int[userGearLength];

            for (int i = 0; i < userGearLength; i++)
            {
                int userGear = binaryReader.ReadInt32();
                gearData[i] = (int)userGear;
            }

            byte[] sendByte = new byte[4 + 4 + 4 + 4 + 4 + 4 + (roomName.Length + 4)];

            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // accepted or not
                    binaryWriterStream.Write((byte)ServerResponse.Accepted);
                    // gamemode
                    binaryWriterStream.Write((byte)GameMode.BattleRoyale);
                    // client requires this, but it's useless
                    binaryWriterStream.Write((byte)1);
                    // player index
                    binaryWriterStream.Write(newIndex);
                    // group index
                    binaryWriterStream.Write((Byte)0);
                    // useless
                    binaryWriterStream.Write(1);
                    // useless string (but using it to notify server of a custom server)
                    binaryWriterStream.Write(Encoding.UTF8.GetBytes("CustomServer"));
                }
            }

            Console.WriteLine("Sending request RESPONSE to client!");
            PacketHandler.SendMessageToPeer(peer, EventCode.RoomInitRequestResponse, sendByte, true);

            Console.WriteLine("Sending Login RESPONSE to client!");
            PacketHandler.SendMessageToPeer(peer, EventCode.Login, SendJoinMessageToServer(newIndex, playerName, gearData, room), true);

            foreach (KeyValuePair<byte, Player> player in room.Players)
            {
                if (player.Key == newIndex)
                {
                    continue;
                }

                // broadcast to ALL players
                player.Value.PendingBroadcastPackets.Add(new TabgPacket(EventCode.Login, SendLoginMessageToServer(newIndex, playerName, gearData)));
            }

            PacketHandler.SendMessageToPeer(peer, EventCode.PlayerDead, room.SendNotification(0, "WELCOME - RUNNING COMMUNITY SERVER V1.TEST"), true);
        }

        private byte[] SendJoinMessageToServer(byte playerIndex, string playerName, int[] gearData, Room room)
        {
            byte[] sendByte = new byte[1024];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // player index
                    binaryWriterStream.Write((Byte)playerIndex);
                    // group index
                    binaryWriterStream.Write((Byte)0);
                    // username length
                    binaryWriterStream.Write((Int32)(playerName.Length));
                    // username
                    binaryWriterStream.Write(Encoding.UTF8.GetBytes(playerName));
                    // is dev
                    binaryWriterStream.Write(true);

                    // set up locations properly
                    Player player = new(playerIndex, 0, playerName, (0f, 200f, 0f), (0f, 0f), gearData);
                    room.AddPlayer(player);

                    // x
                    binaryWriterStream.Write(0f);
                    // y
                    binaryWriterStream.Write(200f);
                    // z

                    binaryWriterStream.Write(0f);
                    // rotation
                    binaryWriterStream.Write((float)0);
                    // is dead
                    binaryWriterStream.Write(false);
                    // is downed
                    binaryWriterStream.Write(false);
                    // health value (not needed)
                    binaryWriterStream.Write((float)100);
                    // something relating to cars? not needed
                    binaryWriterStream.Write(false);
                    // how many players are in the lobby?
                    //binaryWriterStream.Write((byte)concurrencyHandler.Players.Count);
                    binaryWriterStream.Write((byte)(room.Players.Count - 1));

                    // --- OTHER PLAYERS ---

                    foreach (KeyValuePair<byte, Player> item in room.Players)
                    {
                        if (item.Key == playerIndex)
                        {
                            continue;
                        }
                        // player index
                        binaryWriterStream.Write((Byte)item.Key);
                        // group index
                        binaryWriterStream.Write((Byte)0);

                        // convert so bytes can be grabbed
                        byte[] nameBytes = Encoding.UTF8.GetBytes(item.Value.Name);

                        // username length
                        binaryWriterStream.Write((Int32)(nameBytes.Length));
                        // username
                        binaryWriterStream.Write(nameBytes);
                        // gun (this has been disabled for efficiency)
                        binaryWriterStream.Write((Int32)0);

                        // gear data
                        binaryWriterStream.Write((Int32)item.Value.GearData.Length);
                        for (int i = 0; i < item.Value.GearData.Length; i++)
                        {
                            binaryWriterStream.Write((Int32)item.Value.GearData[i]);
                        }

                        // is dev
                        binaryWriterStream.Write(true);
                        // colour (disabled amongus gamemode)
                        binaryWriterStream.Write((Int32)0);
                    }

                    // --- END OTHER PLAYERS ---

                    // --- WEAPONS SECTION ---
                    // number of weapons to spawn, just leave this empty...
                    binaryWriterStream.Write((Int32)0);
                    // --- END WEAPONS SECTION ---

                    // --- CARS SECTION ---

                    // THIS IS CONFUSING AND BROKEN!!!
                    binaryWriterStream.Write((Int32)1);
                    // car id
                    binaryWriterStream.Write((Int32)1);
                    // car index
                    binaryWriterStream.Write((Int32)0);
                    // seats
                    binaryWriterStream.Write((Int32)4);
                    for (int i = 0; i < 4; i++)
                    {
                        binaryWriterStream.Write((Int32)i);
                    }
                    // parts of car
                    binaryWriterStream.Write((byte)4);
                    for (int i = 0; i < 4; i++)
                    {
                        // index
                        binaryWriterStream.Write((byte)i);
                        // health
                        binaryWriterStream.Write(100f);
                        // name
                        binaryWriterStream.Write("Test");
                    }

                    // --- END CARS SECTION ---

                    // time of day
                    binaryWriterStream.Write((float)0);
                    // seconds before first ring
                    binaryWriterStream.Write((float)1000);
                    // base ring time
                    binaryWriterStream.Write((float)1000);
                    // something ring-related, just set to false to disable
                    binaryWriterStream.Write((byte)0);
                    // lives
                    binaryWriterStream.Write((Int32)2);
                    // kills to win
                    binaryWriterStream.Write((ushort)10);
                    // gamestate
                    binaryWriterStream.Write((Byte)GameState.Started);

                    // flying stuff (?)
                    //binaryWriterStream.Write(0f);
                    //binaryWriterStream.Write(200f);
                    //binaryWriterStream.Write(0f);
                    //binaryWriterStream.Write(0f);
                    //binaryWriterStream.Write(200f);
                    //binaryWriterStream.Write(0f);
                }
            }
            return sendByte;
        }

        private byte[] SendLoginMessageToServer(byte playerIndex, string playerName, int[] gearData)
        {
            byte[] sendByte = new byte[1024];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // player index
                    binaryWriterStream.Write((Byte)playerIndex);
                    // group index
                    binaryWriterStream.Write((Byte)0);
                    // username length
                    binaryWriterStream.Write((Int32)(playerName.Length));
                    // username
                    binaryWriterStream.Write(Encoding.UTF8.GetBytes(playerName));

                    // gear data
                    binaryWriterStream.Write((Int32)gearData.Length);
                    for (int i = 0; i < gearData.Length; i++)
                    {
                        binaryWriterStream.Write((Int32)gearData[i]);
                    }

                    // is dev
                    binaryWriterStream.Write(true);
                }
            }
            return sendByte;
        }
    }

    internal class ChatMessagePacketHandler : IPacketHandler // UPDATE THIS NON WORKING
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            byte playerIndex = binaryReader.ReadByte(); // or ReadInt32(), depending on the type of PlayerIndex
            byte messageLength = binaryReader.ReadByte();
            byte[] messageBytes = binaryReader.ReadBytes(messageLength);
            string message = Encoding.Unicode.GetString(messageBytes);
            Console.WriteLine("Player " + playerIndex + ": " + message);
        }
    }

    internal class RequestItemThrowPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            PacketHandler.BroadcastPacket(EventCode.ItemThrown, Room.ClientRequestThrow(binaryReader), room);
        }
    }

    internal class RequestItemDropPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            PacketHandler.BroadcastPacket(EventCode.ItemDrop, Room.ClientRequestDrop(binaryReader, room), room);
        }
    }

    internal class RequestWeaponPickUpPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            PacketHandler.BroadcastPacket(EventCode.WeaponPickUpAccepted, Room.ClientRequestPickUp(binaryReader, room), room);
        }
    }

    internal class PlayerUpdatePacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            // this packet is different because it can have an unlimited amount of subpackets
            UpdatePacket updatePacket = room.PlayerUpdate(binaryReader, buffer.Length, room);

            PacketHandler.SendMessageToPeer(peer, EventCode.PlayerUpdate, updatePacket.Packet, true);

            // have to do this so enumeration is safe
            List<TabgPacket> packetList = updatePacket.BroadcastPackets.PendingBroadcastPackets;

            // also use this packet to send pending broadcast packets
            foreach (TabgPacket packet in packetList)
            {
                PacketHandler.SendMessageToPeer(peer, packet.Type, packet.Data, true);
            }

            updatePacket.BroadcastPackets.PendingBroadcastPackets = new List<TabgPacket>();
        }
    }

    internal class WeaponChangePacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            PacketHandler.BroadcastPacket(EventCode.WeaponChanged, room.PlayerChangedWeapon(binaryReader), room);
        }
    }

    internal class PlayerFirePacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            room.PlayerFire(binaryReader, buffer.Length, room);
        }
    }

    internal class RequestSyncProjectileEventPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            PacketHandler.BroadcastPacket(EventCode.SyncProjectileEvent, room.ClientRequestProjectileSyncEvent(binaryReader, buffer.Length), room);
        }
    }

    internal class RequestAirplaneDropPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            PacketHandler.BroadcastPacket(EventCode.PlayerAirplaneDropped, room.RequestAirplaneDrop(binaryReader), room);
        }
    }

    internal class DamageEventPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            room.PlayerDamagedEvent(binaryReader, room);
        }
    }

    internal class RequestBlessingPacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            PacketHandler.BroadcastPacket(EventCode.BlessingRecieved, room.RequestBlessingEvent(binaryReader), room);
        }
    }

    internal class RequestHealthStatePacketHandler : IPacketHandler
    {
        public void Handle(Peer peer, byte[] buffer, BinaryReader binaryReader, Room room)
        {
            PacketHandler.BroadcastPacket(EventCode.PlayerHealthStateChanged, room.RequestHealthState(binaryReader), room);
        }
    }
}
