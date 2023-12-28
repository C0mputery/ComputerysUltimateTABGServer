# Packets Documentation

## Overview
- Packets are what is sent too and from clients.
- Made to be expandable at runtime for future plugin support.

## [Packet Manager](PacketManager.cs)
- The Packet manager is the main class that handles packets for rooms.

## [Packet Types](PacketTypes)
- PacketTypes holds the functions that are used to handle the different types of packets that can be sent from clients.
- PacketTypes is a partal class broken up into many files to make it easier to read and edit.

## What makes a packet handler
Using the PacketHandlerDelegate delegate.

## Implementing A Tabg Packet
- Find the packet type you want to implement in the [to do](toDo.md) list.
- Make a new file in the [Packet Types](PacketTypes) folder with the name of the packet type (try and follow the existing naming scheme).
- Copy the example packet code into the new file, and change the name of the function to the name of the file.
- Next you need to open up tabg in any c# decomplier (I use [dnSpyEx](https://github.com/dnSpyEx)), and locate the class that sends the packet you want to implement.
- After this point it's effectively guess work (unless you've got landfalls servers), you need to figure out what packet the servers send AND what the server does with the packet.
- Once you figure that out you need to add the function to the PacketHandlers dictonary in the [PacketManager.cs](PacketManager.cs) file, and update the todo list.

## Packet Example
```
using ComputerysUltimateTABGServer.Rooms;
using ENet;

namespace ComputerysUltimateTABGServer.Packets
{
    public static partial class PacketTypes
    {
        public static void ExamplePacket(Peer peer, byte[] receivedPacketRaw, Room room)
        {

        }
    }
}
```