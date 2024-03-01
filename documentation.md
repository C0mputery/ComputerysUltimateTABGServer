# Overview Of The Project
CUTS servers are programed in C#, and utilize the [C# Enet Wraper](https://github.com/nxrighthere/ENet-CSharp) to communicate
with tabg clients.
CUTS (and tabg), work using a client-server model, where the server is the host of the game, and the clients are the players.
<br><br>
CUTS itself operates on a system of "rooms", where each room is a seperate server, and each room holds the data for a single match.
This allows for multiple matches to be hosted of one instance of the program.
Each room runs on it's own thread, and is responsible for handling all of the game logic for the match.

# Documentation For Each Part Of The Project

- [Rooms](Rooms/roomDocumentation.md)
- [Packets](Packets/packetsDocumentation.md)
- [Ticks](Ticks/ticksDocumentation.md)
- [Admin Commands](AdminCommands/adminCommandsDocumentation.md) This is likely to be removed in the future and turned into a plugin.
- [Interface](Interface/interfaceDocumentation.md)
- [Data Types](DataTypes/dataTypesDocumentation.md)
- [Server List](ServerList/serverListDocumentation.md)
- [TABG Code](TABGCode/TABGCodeDocumentation.md)