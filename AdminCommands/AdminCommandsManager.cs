using ComputerysUltimateTABGServer.DataTypes.Player;
using ComputerysUltimateTABGServer.Rooms;
using ENet;
using System.Collections.Frozen;

namespace ComputerysUltimateTABGServer.AdminCommands
{
    // This entire thing feels like a hack, even every command feels like a hack! yippee!
    // Right now this is for testing purposes only, but I have no clue how to do this better so it's staying like this for probably a while.
    public static partial class AdminCommandManager
    {
        // I'm not sure if i like this, but it works for now, I ALSO DON'T KNOW HOW TO DO THIS BETTER!
        public static readonly FrozenDictionary<string, AdminCommandDelegate> UserCommands = new Dictionary<string, AdminCommandDelegate>
        {

        }.ToFrozenDictionary();
        public static readonly FrozenDictionary<string, AdminCommandDelegate> ModeratorCommands = new Dictionary<string, AdminCommandDelegate>
        {

        }.ToFrozenDictionary();
        public static readonly FrozenDictionary<string, AdminCommandDelegate> AdminCommands = new Dictionary<string, AdminCommandDelegate>
        {
            { "give", AdminCommandTypes.AdminCommandGive },
            { "tp", AdminCommandTypes.AdminCommandTP },
            { "testkit", AdminCommandTypes.AdminCommandTestKit },
            { "start", AdminCommandTypes.AdminCommandStartGame },
            { "setgamestate", AdminCommandTypes.AdminCommandSetGameState }

        }.ToFrozenDictionary();
        public static readonly FrozenDictionary<string, AdminCommandDelegate> OwnerCommands = new Dictionary<string, AdminCommandDelegate>
        {

        }.ToFrozenDictionary();

        public static void HandleAdminCommand(Peer peer, byte[] receivedPacketRaw, Room room)
        {
            byte PlayerID = receivedPacketRaw[0];
            byte MessageLength = receivedPacketRaw[1];
            string Message = System.Text.Encoding.Unicode.GetString(receivedPacketRaw, 2, MessageLength);
            if (Message[0] != '/') { return; }

            Message = Message[1..];
            string[] CommandArguments = Message.Split(' ');
            string Command = CommandArguments[0];
            CommandArguments = CommandArguments[1..];
            CommandArguments = CommandArguments.Select(item => item.ToLower()).ToArray();

            if (!room.TryToGetPlayer(PlayerID, out Player? commandSender)) { return; }

            commandSender.m_PermissionLevel = PermissionLevel.Admin; // TODO: Remove this

            AdminCommandDelegate? CommandHandler;
            if (commandSender.m_PermissionLevel >= PermissionLevel.Owner && OwnerCommands.TryGetValue(Command, out CommandHandler))
            {
                CommandHandler(peer, CommandArguments, room); return;
            }
            if (commandSender.m_PermissionLevel >= PermissionLevel.Admin && AdminCommands.TryGetValue(Command, out CommandHandler))
            {
                CommandHandler(peer, CommandArguments, room); return;
            }
            if (commandSender.m_PermissionLevel >= PermissionLevel.Moderator && ModeratorCommands.TryGetValue(Command, out CommandHandler))
            {
                CommandHandler(peer, CommandArguments, room); return;
            }
            if (UserCommands.TryGetValue(Command, out CommandHandler)) { CommandHandler(peer, CommandArguments, room); }
        }

        public delegate void AdminCommandDelegate(Peer peer, string[] CommandArguments, Room room);
    }

    public enum PermissionLevel
    {
        User = 0,
        Moderator = 1,
        Admin = 2,
        Owner = 3
    }
}