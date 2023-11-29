using ENet;
using TABGCommunityServer.Packets;

namespace TABGCommunityServer
{
    class TABGCommunityServer
    {
        static void Main()
        {
            ushort port = 9701;
            ushort maxClients = 256;
            Console.WriteLine("TABG COMMUNITY SERVER v1 STARTED");

            ENet.Library.Initialize();

            using Host enetHost = new Host();

            Address address = new Address();

            address.Port = port;
            address.SetIP("0.0.0.0");

            enetHost.Create(address, maxClients);

            Event netEvent;

            Console.WriteLine("Server Started!");

            while (!Console.KeyAvailable)
            {
                while (enetHost.CheckEvents(out netEvent) > 0 && !(enetHost.Service(15, out netEvent) <= 0))
                {
                    switch (netEvent.Type)
                    {
                        case EventType.None:
                            break;

                        case EventType.Connect:
                            Console.WriteLine("Client connected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                            break;

                        case EventType.Disconnect:
                            Console.WriteLine("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                            break;

                        case EventType.Timeout:
                            Console.WriteLine("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                            break;
                        case EventType.Receive:
                            byte[] enetPacket = new byte[netEvent.Packet.Length];
                            netEvent.Packet.CopyTo(enetPacket);

                            EventCode code = (EventCode)enetPacket[0];
                            byte[] buffer = new byte[enetPacket.Length - 1];
                            Array.Copy(enetPacket, 1, buffer, 0, buffer.Length);

                            PacketHandler.Handle(netEvent.Peer, code, buffer);

                            netEvent.Packet.Dispose();
                            break;
                    }
                }
            }

            enetHost.Flush();

        }
    }
}