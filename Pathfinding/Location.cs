using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    class Location
    {
        public int X;       // X-coordinate
        public int Y;       // Y-coordinate
        public int F;       // Total Score
        public int G;       // Distance from start
        public int H;       // Guestimate distance to end.

        public Location Parent;
    }
}
