using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemLogic
{
    public class SpawnLocation
    {
        private bool hasItem = false;

        double x { get; }
        double y { get; }
        double z { get; }

        public SpawnLocation(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
