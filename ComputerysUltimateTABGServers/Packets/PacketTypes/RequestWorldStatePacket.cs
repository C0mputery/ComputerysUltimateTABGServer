using ComputerysUltimateTABGServer.DataTypes.Items;
using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.DataTypes.Ring;
using ComputerysUltimateTABGServer.DataTypes.Vehicles;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.Text;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void RequestWorldStatePacket(Peer peer, BinaryReader receivedPacketData, Room room)
        {
            byte receivedPlayerID = receivedPacketData.ReadByte();

            if (!room.TryToGetPlayer(receivedPlayerID, out Player? player) || player == null) { return; }

            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(receivedPlayerID);
                binaryWriter.Write(player.m_GroupIndex);
                byte[] playerNameUTF8 = Encoding.UTF8.GetBytes(player.m_Name);
                binaryWriter.Write(playerNameUTF8.Length);
                binaryWriter.Write(playerNameUTF8);
                binaryWriter.Write(player.m_IsDev);
                binaryWriter.Write(player.m_Position.X);
                binaryWriter.Write(player.m_Position.Y);
                binaryWriter.Write(player.m_Position.Z);
                binaryWriter.Write(player.m_Rotation.X);
                binaryWriter.Write(player.m_IsDead);
                binaryWriter.Write(player.m_IsDowned);
                binaryWriter.Write(player.m_Health);
                binaryWriter.Write(player.m_IsInCar);
                if (player.m_IsInCar)
                {
                    binaryWriter.Write(player.m_OccupiedCarId);
                    binaryWriter.Write(player.m_OccupiedSeatId);
                }
                if (room.m_GameMode == GameMode.Deception)
                {
                    //binaryWriter.Write(player.overrideColor) ReadInt32 // imma guess that this was used when somebody shared the same color as somebody else in amgus
                }
                binaryWriter.Write((byte)room.m_Players.Count);
                foreach (Player playerFromRoom in room.m_Players.Values)
                {
                    binaryWriter.Write(playerFromRoom.m_PlayerID);
                    binaryWriter.Write(playerFromRoom.m_GroupIndex);
                    byte[] remotePlayerNameUTF8 = Encoding.UTF8.GetBytes(playerFromRoom.m_Name);
                    binaryWriter.Write(remotePlayerNameUTF8.Length);
                    binaryWriter.Write(remotePlayerNameUTF8);
                    binaryWriter.Write(playerFromRoom.m_CurrentlyHeldWeaponID);
                    binaryWriter.Write(playerFromRoom.m_GearData.Length);
                    foreach (int gearID in playerFromRoom.m_GearData)
                    {
                        binaryWriter.Write(gearID);
                    }
                    binaryWriter.Write(playerFromRoom.m_IsDev);
                    binaryWriter.Write(playerFromRoom.m_Color);
                }
                binaryWriter.Write(room.m_Items.Count);
                foreach (Item item in room.m_Items)
                {
                    binaryWriter.Write(item.WeaponIndex);
                    binaryWriter.Write(item.UniqueIdentifier);
                    binaryWriter.Write(item.Quantity);
                    binaryWriter.Write(item.Position.X);
                    binaryWriter.Write(item.Position.Y);
                    binaryWriter.Write(item.Position.Z);
                }
                binaryWriter.Write(room.m_Vehicles.Count);
                foreach (Vehicle vehicle in room.m_Vehicles.Values)
                {
                    binaryWriter.Write(vehicle.vehicleID);
                    binaryWriter.Write(vehicle.vehicleIndex);
                    binaryWriter.Write(vehicle.seats.Count);
                    foreach (Seat seat in vehicle.seats)
                    {
                        binaryWriter.Write(seat.SeatIndex);
                    }
                    binaryWriter.Write(vehicle.damageableVehicleParts.Count);
                    foreach (DamageableVehiclePart damageableVehiclePart in vehicle.damageableVehicleParts)
                    {
                        binaryWriter.Write(damageableVehiclePart.partIndex);
                        binaryWriter.Write(damageableVehiclePart.partHealth);
                        binaryWriter.Write(damageableVehiclePart.partName);
                    }
                }
                binaryWriter.Write(room.m_TimeOfDay);
                binaryWriter.Write(room.m_TimeBeforeFirstRing);
                binaryWriter.Write(room.m_BaseRingTime);
                binaryWriter.Write((byte)room.m_RingLocations.Count);
                foreach (RingLocations ringLocations in room.m_RingLocations)
                {
                    binaryWriter.Write(ringLocations.ringSize);
                    binaryWriter.Write(ringLocations.ringSpeed);
                }
                binaryWriter.Write(room.m_PlayerLives);
                binaryWriter.Write(room.m_KillsToWin);
                binaryWriter.Write((byte)room.m_GameState);
                if (room.m_GameState == GameState.CountDown)
                {
                    binaryWriter.Write(room.m_CountDownTimer);
                }
                if (room.m_GameState == GameState.Flying && room.m_GameMode != GameMode.Test)
                {
                    binaryWriter.Write(true); // Should be flying I don't know when this would be the case other than when the gamestate is flying? perhaps the players need a bool to say if theyve dropped or not.
                    binaryWriter.Write(room.m_DropperStart.X);
                    binaryWriter.Write(room.m_DropperStart.Y);
                    binaryWriter.Write(room.m_DropperStart.Z);
                    binaryWriter.Write(room.m_DropperEnd.X);
                    binaryWriter.Write(room.m_DropperEnd.Y);
                    binaryWriter.Write(room.m_DropperEnd.Z);
                }
                PacketHandler.SendPacketToPlayer(EventCode.Login, memoryStream.ToArray(), player, room);
            }
        }
    }
}