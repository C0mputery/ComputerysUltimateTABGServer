using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerysUltimateTABGServer.Misc
{
    public static class RandomNumber
    {
        private static Random m_Random = new Random();
        public static int Next(int min, int max)
        {
            return m_Random.Next(min, max);
        }
    }
}
