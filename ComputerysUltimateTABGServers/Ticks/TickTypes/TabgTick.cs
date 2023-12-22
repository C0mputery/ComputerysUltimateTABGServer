using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.DataTypes.Vehicles;
using ComputerysUltimateTABGServer.Interface.Logging;
using ComputerysUltimateTABGServer.Packets;
using ComputerysUltimateTABGServer.Rooms;
using ComputerysUltimateTABGServer.TABGCode;
using System.Numerics;

namespace ComputerysUltimateTABGServer.Ticks
{
    public static partial class TickTypes
    {
        public static void TabgTick(Room room)
        {
            foreach (Player player in room.m_Players.Values)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(0f); // This is handled on the timeStamp on the client, we have it as zero for now.

                    // The client will leave players were they were when they were last updated!
                    // We need a way to handle this, so sometimes we will need to update all players regardless of distance.
                    // perhaps we could tell the clients that they are in the air, or something like that.
                    // rn this is okay, but we will need to fix this later.
                    IEnumerable<Player> playersInRange = FindPlayersInRangeOfUpdate(player, room);
                    IEnumerable<Vehicle> VehiclesInRange = FindCarsInRangeOfUpdate(player, room);

                    binaryWriter.Write((byte)playersInRange.Count());
                    foreach (Player playerInRange in playersInRange) { WritePlayerUpdate(playerInRange, room, binaryWriter); }

                    binaryWriter.Write((byte)VehiclesInRange.Count());
                    foreach (Vehicle VehicleInRange in VehiclesInRange) { WriteCarUpdate(VehicleInRange, room, binaryWriter); }

                    binaryWriter.Write((byte)0); // idk what this is for, but it is a byte discarded by the client.
                    PacketHandler.SendPacketToPlayer(EventCode.PlayerUpdate, memoryStream.ToArray(), player, room);
                }
            }

            // Make these max out at 255, and then send multiple packets if needed.
            static IEnumerable<Player> FindPlayersInRangeOfUpdate(Player player, Room room)
            {
                return room.m_Players.Values.Where(playerToCheckDistance => player != playerToCheckDistance && Vector3.Distance(playerToCheckDistance.m_Position, player.m_Position) <= room.m_TickUpdateRange);
            }
            static IEnumerable<Vehicle> FindCarsInRangeOfUpdate(Player player, Room room)
            {
                return room.m_Vehicles.Values.Where(carToCheckDistance => Vector3.Distance(carToCheckDistance.position, player.m_Position) <= room.m_TickUpdateRange);
            }

            static void WritePlayerUpdate(Player player, Room room, BinaryWriter binaryWriter)
            {
                binaryWriter.Write(player.m_PlayerID);
                binaryWriter.Write((byte)PacketContainerFlags.All);
                binaryWriter.Write((byte)player.m_DrivingState);
                if (player.m_DrivingState == DrivingState.Driving)
                {
                    room.m_Vehicles.TryGetValue(player.m_OccupiedCarId, out Vehicle vehicle);
                    binaryWriter.Write(vehicle.position.X);
                    binaryWriter.Write(vehicle.position.Y);
                    binaryWriter.Write(vehicle.position.Z);
                    binaryWriter.Write(NetworkOptimizationHelper.OptimizeQuaternion(vehicle.rotation)); // I am guessing these can be removed once we have real car stuff since the clients send it I think 
                    binaryWriter.Write(NetworkOptimizationHelper.OptimizeDirection(vehicle.carInput)); // I am guessing these can be removed once we have real car stuff since the clients send it I think 
                    binaryWriter.Write(player.m_Rotation.X);
                    binaryWriter.Write(player.m_Rotation.Y);
                    binaryWriter.Write((byte)player.m_DrivingState);
                }
                else
                {
                    binaryWriter.Write(player.m_Position.X);
                    binaryWriter.Write(player.m_Position.Y);
                    binaryWriter.Write(player.m_Position.Z);
                    binaryWriter.Write(player.m_Rotation.X);
                    binaryWriter.Write(player.m_Rotation.Y);
                    binaryWriter.Write(player.m_AimingDownSights);
                    binaryWriter.Write(player.m_MovmentDirction);
                    binaryWriter.Write(player.m_MovementFlags);
                }
            }
            static void WriteCarUpdate(Vehicle vehicle, Room room, BinaryWriter binaryWriter)
            {
                binaryWriter.Write(vehicle.vehicleIndex);
                binaryWriter.Write(vehicle.position.X);
                binaryWriter.Write(vehicle.position.Y);
                binaryWriter.Write(vehicle.position.Z);
                binaryWriter.Write(NetworkOptimizationHelper.OptimizeQuaternion(vehicle.rotation)); // I am guessing these can be removed once we have real car stuff since the clients send it I think 
            }
        }
    }
}