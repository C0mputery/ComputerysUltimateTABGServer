using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABGCommunityServer.MiscDataTypes;

namespace ComputerysUltimateTABGServers.MiscDataTypes
{
    public class PlayerGroup(uint groupID = 0, bool openToAutoTeam = false)
    {
        public bool m_OpenToAutoTeam = openToAutoTeam;
        public uint m_GroupID = groupID;
        public List<uint> m_PlayersInGroup = new List<uint>();

        public void addPlayer(Player player)
        {
            m_PlayersInGroup.Add(player.m_Peer.ID);
        }

        public void removePlayer(Player player)
        {
            m_PlayersInGroup.Remove(player.m_Peer.ID);
        }
    }
}
