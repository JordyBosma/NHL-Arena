using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldObjects;

namespace ItemLogic
{
    public class SpawnLocation
    {
        private bool _hasChanged = true;
        private string _itemType;
        private Item _item;

        public Item item { get { return _item; } }
        public string itemType { get { return _itemType; } }
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

        public void addItem(Item item)
        {
            this._item = item;
            _hasChanged = true;
        }

        public void dellItem()
        {
            this._item = null;
            _hasChanged = true;
        }

        public void setChanged()
        {
            _hasChanged = false;
        }
    }
}
