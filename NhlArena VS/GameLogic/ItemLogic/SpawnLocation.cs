using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemLogic
{
    public class SpawnLocation
    {
        private bool _hasItem = false;
        private bool _hasChanged = false;

        public string itemType { get; }
        public bool hasItem { get { return _hasItem; } }
        public bool hasChanged { get { return _hasChanged; } }

        double x { get; }
        double y { get; }
        double z { get; }

        public SpawnLocation(double x, double y, double z, string itemType)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            this.itemType = itemType;
        }

        public void setItem()
        {
            _hasItem = !hasItem;
            _hasChanged = true;
        }

        public void setChanged()
        {
            _hasChanged = false;
        }
    }
}
