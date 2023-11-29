using ENet;
using TABGCommunityServer.Packets;

namespace TABGCommunityServer
{
    class TABGCommunityServer
    {
        static ushort enetPort = 9701;
        static ushort maxEnetClients = 256;
        static Host? enetHost;
        static Address enetAddress;
        static Event enetEvent;

        static void Main()
        {
            Console.WriteLine("TABG COMMUNITY SERVER v1 BOOTING");

            ENet.Library.Initialize();
            enetHost = new Host();
            enetAddress = new Address() { Port = enetPort };
            enetAddress.SetIP("0.0.0.0");
            enetHost.Create(enetAddress, maxEnetClients);

            Console.WriteLine("Server Started!");

            while (!Console.KeyAvailable)
            {
                while (enetHost.CheckEvents(out enetEvent) > 0 && !(enetHost.Service(15, out enetEvent) <= 0))
                {
                    switch (enetEvent.Type)
                    {
                        case EventType.None:
                            break;

                        case EventType.Connect:
                            Console.WriteLine("Client connected - ID: " + enetEvent.Peer.ID + ", IP: " + enetEvent.Peer.IP);
                            break;

                        case EventType.Disconnect:
                            Console.WriteLine("Client disconnected - ID: " + enetEvent.Peer.ID + ", IP: " + enetEvent.Peer.IP);
                            break;

                        case EventType.Timeout:
                            Console.WriteLine("Client timeout - ID: " + enetEvent.Peer.ID + ", IP: " + enetEvent.Peer.IP);
                            break;
                        case EventType.Receive:
                            byte[] enetPacket = new byte[enetEvent.Packet.Length];
                            enetEvent.Packet.CopyTo(enetPacket);

                            EventCode code = (EventCode)enetPacket[0];
                            byte[] buffer = new byte[enetPacket.Length - 1];
                            Array.Copy(enetPacket, 1, buffer, 0, buffer.Length);

                            PacketHandler.Handle(enetEvent.Peer, code, buffer);

                            enetEvent.Packet.Dispose();
                            break;
                    }
                }
            }

            enetHost.Flush();

        }
    }
}