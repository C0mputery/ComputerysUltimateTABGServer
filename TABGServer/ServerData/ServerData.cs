using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TABGCommunityServer.ServerData
{
    internal struct ServerData
    {
        public Dictionary<int, Player> Players = new Dictionary<int, Player>();
        public byte LastID = 0;

        public ServerData()
        {
        }
    }
}
