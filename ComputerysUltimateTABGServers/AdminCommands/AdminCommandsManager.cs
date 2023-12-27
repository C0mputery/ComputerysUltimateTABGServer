using ComputerysUltimateTABGServer.Packets;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.Collections.Frozen;

namespace ComputerysUltimateTABGServer.AdminCommands
{
    public static partial class AdminCommandManager
    {
        public static readonly FrozenDictionary<string, AdminCommandDelegate> PacketHandlers = new Dictionary<string, AdminCommandDelegate>
        {
            
        }.ToFrozenDictionary();

        public static void HandleAdminCommand(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            byte PlayerID = receivedPacketRaw[0];
            byte MessageLength = receivedPacketRaw[1];
            string Message = System.Text.Encoding.UTF8.GetString(receivedPacketRaw, 2, MessageLength);
            if (Message[0] != '/') { return; }

            Message = Message[1..];
            string[] CommandParts = Message.Split(' ');
            string Command = CommandParts[0];
            CommandParts = CommandParts[1..];

            if (!PacketHandlers.TryGetValue(Command, out AdminCommandDelegate? CommandHandler)) { return; } // Send something back to the player saying the command was not found

        }

        public delegate bool AdminCommandDelegate(Peer peer, string[] CommandParts, Room room);
    }
}