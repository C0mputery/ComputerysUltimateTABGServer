using System.Text;
using TABGCommunityServer.MiscDataTypes;

namespace TABGCommunityServer.Rooms
{
    public partial class Room
    {
        public Dictionary<byte, Player> Players { get; private set; } = new Dictionary<byte, Player>();
        public byte LastID = 0;

        public void AddPlayer(Player player)
        {
            Players[player.Id] = player;
            LastID++;
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player.Id);
        }

        public byte[] KillPlayer(int victim, int killer, string victimName)
        {
            byte[] sendByte = new byte[128];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // player index
                    binaryWriterStream.Write((Byte)victim);
                    // player who killed the player
                    binaryWriterStream.Write((Byte)killer);
                    // spectator value: 255 = don't respawn. 254 = go to boss. other ints = spectate that player 
                    binaryWriterStream.Write((Byte)254);
                    // victim length
                    binaryWriterStream.Write((Int32)(victimName.Length));
                    // victim
                    binaryWriterStream.Write(Encoding.UTF8.GetBytes(victimName));
                    // weapon id for killscreen (unused for now)
                    binaryWriterStream.Write((Int32)1);
                    // is ring death (unused for now because ring isn't operational)
                    binaryWriterStream.Write(false);
                }
            }
            return sendByte;
        }

        public byte[] SendNotification(int playerIndex, string notification)
        {
            byte[] sendByte = new byte[128];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // player index (set to 255 for invalid player so nobody gets killed)
                    binaryWriterStream.Write((Byte)255);
                    // player who killed the player
                    binaryWriterStream.Write((Byte)playerIndex);
                    // spectator value is unneeded when killing other players
                    binaryWriterStream.Write((Byte)255);

                    // get amount of bytes
                    byte[] message = Encoding.Unicode.GetBytes(notification);

                    // victim length
                    binaryWriterStream.Write((Byte)(message.Length));
                    // victim
                    binaryWriterStream.Write(message);

                    // weapon id for killscreen (unused for now)
                    binaryWriterStream.Write((Int32)1);
                    // is ring death (unused for now because ring isn't operational)
                    binaryWriterStream.Write(false);
                }
            }
            return sendByte;
        }

        public byte[] GiveItem(int itemID, int amount)
        {
            byte[] sendByte = new byte[128];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // num of items (loops)
                    binaryWriterStream.Write((ushort)1);
                    // item id
                    binaryWriterStream.Write((Int32)itemID);
                    // quantity of item
                    binaryWriterStream.Write((Int32)amount);
                    // Client requires this, but it's redundant
                    binaryWriterStream.Write((Int32)0);
                }
            }
            return sendByte;
        }

        public static byte[] GiveGear()
        {
            byte[] sendByte = new byte[768];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // number of loops
                    binaryWriterStream.Write((ushort)14);

                    // start with the ammo
                    for (int i = 0; i < 10; i++)
                    {
                        // item id
                        binaryWriterStream.Write((Int32)i);
                        // quantity of item
                        binaryWriterStream.Write((Int32)999);
                    }

                    // give LP grenade
                    binaryWriterStream.Write((Int32)202);
                    binaryWriterStream.Write((Int32)1);

                    // give vector
                    binaryWriterStream.Write((Int32)315);
                    binaryWriterStream.Write((Int32)999);

                    // give AWP
                    binaryWriterStream.Write((Int32)317);
                    binaryWriterStream.Write((Int32)1);

                    // give deagle
                    binaryWriterStream.Write((Int32)266);
                    binaryWriterStream.Write((Int32)1);

                    // Client requires this, but it's redundant
                    binaryWriterStream.Write((Int32)0);
                }
            }
            return sendByte;
        }

        public UpdatePacket PlayerUpdate(BinaryReader binaryReader, int packetLength, Room room)
        {
            // player
            byte playerIndex = binaryReader.ReadByte();

            // location
            float x = binaryReader.ReadSingle();
            float y = binaryReader.ReadSingle();
            float z = binaryReader.ReadSingle();

            // rotation
            float rotX = binaryReader.ReadSingle();
            float rotY = binaryReader.ReadSingle();

            // "ads" bool (?)
            // OHH THIS MEANS AIM DOWN SIGHTS
            bool ads = binaryReader.ReadBoolean();

            // optimizeDirection (?)
            int arrayLen = packetLength - 23;
            byte[] optimizeDirection = binaryReader.ReadBytes(arrayLen);

            // movement flags
            byte movementFlags = binaryReader.ReadByte();

            Player player = room.Players[playerIndex];

            player.Id = playerIndex;
            player.Location = (x, y, z);
            player.Rotation = (rotX, rotY);
            player.Ads = ads;
            player.OptimizedDirection = optimizeDirection;
            player.MovementFlags = movementFlags;

            UpdatePacket fullPacket = new UpdatePacket(SendPlayerUpdateResponsePacket(room), player);

            return fullPacket;
        }

        public void PlayerFire(BinaryReader binaryReader, int length, Room room)
        {
            // player index
            byte playerIndex = binaryReader.ReadByte();
            // firing mode
            byte firingMode = binaryReader.ReadByte();
            // ammo type
            int ammoType = binaryReader.ReadInt32();

            // extra data (vectors and more)
            // you gotta be kidding me, did Landfall really just put a flag in rather than passing at least idk like an EMPTY ARRAY?
            // WTF LANDFALL
            int extraDataLength = length - 6;
            byte[] extraData = new byte[0];

            if (extraDataLength != 0)
            {
                extraData = binaryReader.ReadBytes(extraDataLength);
            }

            FiringMode firingModeFlag = (FiringMode)firingMode;
            byte[] sendByte = new byte[1024];
            try
            {
                using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
                {
                    using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                    {

                        // ANOTHER binary stream because of Landfall's bad design
                        // i KNOW this is confusing but the variable scopes require me to write it this way
                        using (MemoryStream memoryStream = new MemoryStream(extraData))
                        {
                            using (BinaryReader extraDataBinaryReader = new BinaryReader(memoryStream))
                            {
                                binaryWriterStream.Write(playerIndex);
                                binaryWriterStream.Write(firingMode);
                                binaryWriterStream.Write(ammoType);

                                // flag is used to signal firing mode
                                if ((firingModeFlag & FiringMode.ContainsDirection) == FiringMode.ContainsDirection)
                                {
                                    // vector direction
                                    float x = extraDataBinaryReader.ReadSingle();
                                    float y = extraDataBinaryReader.ReadSingle();
                                    float z = extraDataBinaryReader.ReadSingle();

                                    binaryWriterStream.Write(x);
                                    binaryWriterStream.Write(y);
                                    binaryWriterStream.Write(z);

                                    // random patch!!!
                                    // THIS CRASHES THE SERVER
                                    // IF YOU USE GUST SPELL!
                                    while (binaryReader.PeekChar() != -1)
                                    {
                                        byte quaternion = binaryReader.ReadByte();
                                        binaryWriterStream.Write(quaternion);
                                        if (!(quaternion >= 4))
                                        {
                                            byte[] quaternionData = extraDataBinaryReader.ReadBytes(6);
                                            binaryWriterStream.Write(quaternionData);
                                        }
                                    }
                                }

                                if ((firingModeFlag & FiringMode.WantsToBeSynced) == FiringMode.WantsToBeSynced)
                                {
                                    int syncIndex = binaryReader.ReadInt32();
                                    binaryWriterStream.Write(syncIndex);
                                }

                                if ((firingModeFlag & FiringMode.UseBulletEffect) == FiringMode.UseBulletEffect)
                                {
                                    byte bulletEffectType = binaryReader.ReadByte();
                                    binaryWriterStream.Write(bulletEffectType);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }

            foreach (KeyValuePair<byte, Player> item in room.Players)
            {
                if (item.Key == playerIndex)
                {
                    continue;
                }

                // broadcast to ALL players but the shooter
                item.Value.PendingBroadcastPackets.Add(new TabgPacket(EventCode.PlayerFire, sendByte));
            }

            //return sendByte;
        }

        public byte[] PlayerChangedWeapon(BinaryReader binaryReader)
        {
            // player index
            byte playerIndex = binaryReader.ReadByte();
            // slot flag
            byte slotFlag = binaryReader.ReadByte();

            // UNKNOWN W VALUES
            // w1
            short w1 = binaryReader.ReadInt16();
            // w2
            short w2 = binaryReader.ReadInt16();
            // w3
            short w3 = binaryReader.ReadInt16();
            // w4
            short w4 = binaryReader.ReadInt16();
            // w5
            short w5 = binaryReader.ReadInt16();

            // attachments
            byte attachmentsLength = binaryReader.ReadByte();
            short[] attachments = new short[256];
            for (int i = 0; i < attachmentsLength; i++)
            {
                short attachmentID = binaryReader.ReadInt16();
                attachments[i] = (short)attachmentID;
            }

            // is throwable
            short throwable = binaryReader.ReadInt16();

            byte[] sendByte = new byte[2048];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // player index
                    binaryWriterStream.Write(playerIndex);
                    binaryWriterStream.Write(slotFlag);
                    binaryWriterStream.Write(w1);
                    binaryWriterStream.Write(w2);
                    binaryWriterStream.Write(w3);
                    binaryWriterStream.Write(w4);
                    binaryWriterStream.Write(w5);
                    binaryWriterStream.Write(attachments.Length);
                    for (int i = 0; i < attachments.Length; i++)
                    {
                        binaryWriterStream.Write(attachments[i]);
                    }
                    binaryWriterStream.Write(throwable);
                }
            }

            return sendByte;
        }

        public byte[] SendPlayerUpdateResponsePacket(Room room)
        {
            byte[] sendByte = new byte[2048];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // ms (to get ms since last update)
                    float milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    binaryWriterStream.Write(milliseconds);

                    // amount of players to loop (unimplemented)
                    binaryWriterStream.Write((byte)room.Players.Count);

                    foreach (KeyValuePair<byte, Player> item in room.Players)
                    {
                        // player index
                        binaryWriterStream.Write((byte)item.Key);

                        // packet container flags
                        PacketContainerFlags packetContainerFlags = PacketContainerFlags.PlayerPosition | PacketContainerFlags.PlayerRotation | PacketContainerFlags.PlayerDirection;
                        binaryWriterStream.Write((byte)packetContainerFlags);

                        // driving state
                        binaryWriterStream.Write((byte)DrivingState.None);

                        // player position (triggered by packet container flag)
                        (float X, float Y, float Z) loc = item.Value.Location;
                        binaryWriterStream.Write(loc.X);
                        binaryWriterStream.Write(loc.Y);
                        binaryWriterStream.Write(loc.Z);

                        // player rotation (triggered by flag)
                        (float X, float Y) rot = item.Value.Rotation;
                        binaryWriterStream.Write(rot.X);
                        binaryWriterStream.Write(rot.Y);

                        // aim down sights state
                        binaryWriterStream.Write(item.Value.Ads);

                        // optimized direction
                        binaryWriterStream.Write(item.Value.OptimizedDirection);

                        // movement flag bytes
                        binaryWriterStream.Write(item.Value.MovementFlags);
                    }

                    // car stuff - vehicles are disabled so this isn't important
                    binaryWriterStream.Write((byte)0);
                }
            }
            return sendByte;
        }

        public byte[] RequestAirplaneDrop(BinaryReader binaryReader)
        {
            // player index
            byte index = binaryReader.ReadByte();

            // location
            float x = binaryReader.ReadSingle();
            float y = binaryReader.ReadSingle();
            float z = binaryReader.ReadSingle();

            byte[] sendByte = new byte[128];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    binaryWriterStream.Write(index);
                    binaryWriterStream.Write(x);
                    binaryWriterStream.Write(y);
                    binaryWriterStream.Write(z);
                    binaryWriterStream.Write(x - 1);
                    binaryWriterStream.Write(y - 100);
                    binaryWriterStream.Write(z - 1);
                }
            }

            return sendByte;
        }

        public byte[] RevivePlayer(byte playerID)
        {
            byte[] sendByte = new byte[128];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    binaryWriterStream.Write((byte)ReviveState.Finished);
                    // player being revived
                    binaryWriterStream.Write(playerID);
                    // player who is revivng
                    binaryWriterStream.Write((byte)255);
                }
            }
            return sendByte;
        }

        public byte[] ClientRequestProjectileSyncEvent(BinaryReader binaryReader, int fullLength)
        {
            // "sync index"
            int syncIndex = binaryReader.ReadInt32();
            // removed
            bool removed = binaryReader.ReadBoolean();
            // "everyone"
            bool everyone = binaryReader.ReadBoolean();
            // include self (?)
            bool includeSelf = binaryReader.ReadBoolean();
            // is static (?)
            bool isStatic = binaryReader.ReadBoolean();

            // additional data length
            int addDataLength = fullLength - 8;
            // additional data byte
            byte[] additionalData = binaryReader.ReadBytes(addDataLength);

            using (MemoryStream memoryStream = new MemoryStream(additionalData))
            {
                using (BinaryReader extraDataBinaryReader = new BinaryReader(memoryStream))
                {
                    byte syncProjectileDataType = (byte)0;
                    while (binaryReader.PeekChar() != -1)
                    {
                        syncProjectileDataType = extraDataBinaryReader.ReadByte();
                    }
                    byte[] sendByte = new byte[128];
                    using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
                    {
                        using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                        {
                            binaryWriterStream.Write(syncIndex);
                            // removed flag
                            binaryWriterStream.Write(removed);
                            // these don't matter
                            binaryWriterStream.Write(everyone);
                            binaryWriterStream.Write(includeSelf);
                            binaryWriterStream.Write(isStatic);

                            // case
                            switch (syncProjectileDataType)
                            {
                                case 1:
                                    {
                                        int syncedInt = extraDataBinaryReader.ReadInt32();
                                        binaryWriterStream.Write(syncedInt);
                                        break;
                                    }
                                case 3:
                                    {
                                        float x = extraDataBinaryReader.ReadSingle();
                                        float y = extraDataBinaryReader.ReadSingle();
                                        float z = extraDataBinaryReader.ReadSingle();

                                        binaryWriterStream.Write(x);
                                        binaryWriterStream.Write(y);
                                        binaryWriterStream.Write(z);
                                        break;
                                    }
                                case 4:
                                    {
                                        float x = extraDataBinaryReader.ReadSingle();
                                        float y = extraDataBinaryReader.ReadSingle();
                                        float z = extraDataBinaryReader.ReadSingle();

                                        float x2 = extraDataBinaryReader.ReadSingle();
                                        float y2 = extraDataBinaryReader.ReadSingle();
                                        float z2 = extraDataBinaryReader.ReadSingle();

                                        binaryWriterStream.Write(x);
                                        binaryWriterStream.Write(y);
                                        binaryWriterStream.Write(z);
                                        binaryWriterStream.Write(x2);
                                        binaryWriterStream.Write(y2);
                                        binaryWriterStream.Write(z2);
                                        break;
                                    }
                                case 5:
                                    {
                                        // no idea what this does... client requires it tho
                                        float randomFloat = extraDataBinaryReader.ReadSingle();
                                        binaryWriterStream.Write(randomFloat);
                                        break;
                                    }
                                case 6:
                                    {
                                        bool flag2 = extraDataBinaryReader.ReadByte() == 1;
                                        binaryWriterStream.Write(flag2);
                                        break;
                                    }
                                case 7:
                                    {
                                        // two bytes for collission
                                        byte b = extraDataBinaryReader.ReadByte();
                                        byte b2 = extraDataBinaryReader.ReadByte();

                                        binaryWriterStream.Write(b);
                                        binaryWriterStream.Write(b2);
                                        break;
                                    }
                                case 8:
                                    {
                                        // collision bytes PLUS vector
                                        byte b = extraDataBinaryReader.ReadByte();
                                        byte b2 = extraDataBinaryReader.ReadByte();

                                        float x = extraDataBinaryReader.ReadSingle();
                                        float y = extraDataBinaryReader.ReadSingle();
                                        float z = extraDataBinaryReader.ReadSingle();

                                        binaryWriterStream.Write(b);
                                        binaryWriterStream.Write(b2);
                                        binaryWriterStream.Write(x);
                                        binaryWriterStream.Write(y);
                                        binaryWriterStream.Write(z);
                                        break;
                                    }
                                case 9:
                                    {
                                        // just one byte (?)
                                        byte b5 = extraDataBinaryReader.ReadByte();

                                        binaryWriterStream.Write(b5);
                                        break;
                                    }
                                case 10:
                                    {
                                        // one byte and one bool. not sure what this does either
                                        byte b6 = extraDataBinaryReader.ReadByte();
                                        bool flag3 = extraDataBinaryReader.ReadBoolean();

                                        binaryWriterStream.Write(b6);
                                        binaryWriterStream.Write(flag3);
                                        break;
                                    }
                            }
                        }
                    }
                    return sendByte;
                }
            }
        }

        public byte[] RequestBlessingEvent(BinaryReader binaryReader)
        {
            // player index
            byte playerIndex = binaryReader.ReadByte();
            // blessing slots
            int slot1 = binaryReader.ReadInt32();
            int slot2 = binaryReader.ReadInt32();
            int slot3 = binaryReader.ReadInt32();

            byte[] sendByte = new byte[256];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    binaryWriterStream.Write(playerIndex);
                    binaryWriterStream.Write(slot1);
                    binaryWriterStream.Write(slot2);
                    binaryWriterStream.Write(slot3);
                }
            }
            return sendByte;
        }

        public void PlayerDamagedEvent(BinaryReader binaryReader, Room room)
        {
            byte[] sendByte = new byte[256];
            Player playerOutside;
            Player playerOutside2;
            byte attackerOutside;

            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // attacker id
                    byte attacker = binaryReader.ReadByte();
                    attackerOutside = attacker;

                    // victim id
                    byte victim = binaryReader.ReadByte();
                    Player player = room.Players[victim];
                    playerOutside = player;

                    Player player2 = room.Players[attacker];
                    playerOutside2 = player;

                    binaryWriterStream.Write(victim);
                    binaryWriterStream.Write(attacker);

                    // health
                    float health = binaryReader.ReadSingle();
                    binaryWriterStream.Write(health);
                    player.Health = health;

                    Console.WriteLine("Attacker: " + attacker + ". Victim: " + victim + ". New health value: " + health);

                    // direction
                    float x = binaryReader.ReadSingle();
                    float y = binaryReader.ReadSingle();
                    float z = binaryReader.ReadSingle();
                    binaryWriterStream.Write(x);
                    binaryWriterStream.Write(y);
                    binaryWriterStream.Write(z);

                    // "flag" for force
                    bool flag = binaryReader.ReadBoolean();
                    binaryWriterStream.Write((byte)1);
                    if (flag)
                    {
                        float forceX = binaryReader.ReadSingle();
                        float forceY = binaryReader.ReadSingle();
                        float forceZ = binaryReader.ReadSingle();
                        byte rigIndex = binaryReader.ReadByte();
                        byte forceMode = binaryReader.ReadByte();
                        binaryWriterStream.Write(forceX);
                        binaryWriterStream.Write(forceY);
                        binaryWriterStream.Write(forceZ);
                        binaryWriterStream.Write(rigIndex);
                        binaryWriterStream.Write(forceMode);
                    }
                    else
                    {
                        bool returnToSender = binaryReader.ReadBoolean();
                    }
                }
            }

            playerOutside.PendingBroadcastPackets.Add(new TabgPacket(EventCode.PlayerDamaged, sendByte));
            playerOutside2.PendingBroadcastPackets.Add(new TabgPacket(EventCode.PlayerDamaged, sendByte));

            //foreach (var item in PlayerConcurencyHandler.Players)
            //{
            //    if (item.Key == attackerOutside)
            //    {
            //        continue;
            //    }

            // broadcast to ALL players but the shooter
            //    item.Value.PendingBroadcastPackets.Add(new Packet(ClientEventCode.PlayerDamaged, sendByte));
            //}

            return;
        }

        public byte[] SetPlayerHealth(byte playerID, float newHealth)
        {
            byte[] sendByte = new byte[128];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    binaryWriterStream.Write(playerID);
                    binaryWriterStream.Write(newHealth);
                }
            }
            return sendByte;
        }

        public static byte[] SimulateChunkEnter(byte playerIndex, TABGPlayerState playerState, float health, Room room)
        {
            byte[] sendByte = new byte[1024];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // player index
                    binaryWriterStream.Write(playerIndex);
                    // player state (transcended etc)
                    binaryWriterStream.Write((byte)playerState);
                    // health
                    binaryWriterStream.Write(health);
                    // is player downed
                    binaryWriterStream.Write(false);
                    // is skydiving
                    binaryWriterStream.Write(false);
                    // gear data
                    Player player = room.Players[playerIndex];
                    binaryWriterStream.Write((Int32)player.GearData.Length);
                    for (int i = 0; i < player.GearData.Length; i++)
                    {
                        binaryWriterStream.Write(player.GearData[i]);
                    }
                    // equipment (not tracked by concurrency handler yet)
                    binaryWriterStream.Write((byte)0);
                    // attachments (also not tracked)
                    binaryWriterStream.Write((byte)0);
                    // blessings
                    binaryWriterStream.Write((Int32)0);
                    binaryWriterStream.Write((Int32)0);
                    binaryWriterStream.Write((Int32)0);
                    // should spawn car?
                    binaryWriterStream.Write((byte)DrivingState.None);
                }
            }
            return sendByte;
        }

        public byte[] RequestHealthState(BinaryReader binaryReader)
        {

            // player's index
            byte playerIndex = binaryReader.ReadByte();
            // new health
            float newHealth = binaryReader.ReadSingle();

            // same packet as heal
            return SetPlayerHealth(playerIndex, newHealth);
        }

        public byte[] BroadcastPlayerJoin(byte playerID, string playerName, int[] gearData)
        {
            byte[] sendByte = new byte[128];
            using (MemoryStream writerMemoryStream = new MemoryStream(sendByte))
            {
                using (BinaryWriter binaryWriterStream = new BinaryWriter(writerMemoryStream))
                {
                    // player id
                    binaryWriterStream.Write(playerID);

                    // group id
                    binaryWriterStream.Write((byte)0);

                    // username length
                    binaryWriterStream.Write((Int32)(playerName.Length));
                    // username
                    binaryWriterStream.Write(Encoding.UTF8.GetBytes(playerName));

                    // gear data.
                    binaryWriterStream.Write(gearData.Length);
                    foreach (int num in gearData)
                    {
                        binaryWriterStream.Write(num);
                    }

                    // is dev
                    binaryWriterStream.Write(true);
                }
            }
            return sendByte;
        }
    }
}
