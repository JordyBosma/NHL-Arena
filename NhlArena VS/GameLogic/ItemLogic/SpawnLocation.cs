using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemLogic
{
    public class SpawnLocation
    {
        private bool _hasItem = false;
        private bool _hasChanged = true;
        private string _itemType;

        public string itemType { get { return _itemType; } }
        public bool hasItem { get { return _hasItem; } }
        public bool hasChanged { get { return _hasChanged; } }

        public double x { get; }
        public double y { get; }
        public double z { get; }

        public SpawnLocation(double x, double y, double z, string itemType)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            this._itemType = itemType;
        }

        public void changeItem(string newItemType)
        {
            this._itemType = newItemType;
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
