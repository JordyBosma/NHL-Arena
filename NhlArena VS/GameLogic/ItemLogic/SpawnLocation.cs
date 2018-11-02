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

    public class PlayerSpawnLocationList
    {
        private List<PlayerSpawnLocation> locations = new List<PlayerSpawnLocation>();
        private int listItemCount = 0;
        
        public PlayerSpawnLocationList()
        {

        }

        public void AddLocation(double x, double y, double z, double rotationY)
        {
            locations.Add(new PlayerSpawnLocation(x, y, z, rotationY));
            listItemCount++;
        }
        
        public PlayerSpawnLocation GetSpawnLocation()
        {
            Random rnd = new Random();
            return locations[rnd.Next(listItemCount)];
        }
    }

    public class PlayerSpawnLocation : Object3D
    {
        private List<PlayerSpawnLocation> locations = new List<PlayerSpawnLocation>();

        public PlayerSpawnLocation(double x, double y, double z, double rotationY) : base(x, y, z, 0, rotationY, 0, "PlayerSpawnLocation")
        {

        }
    }
}
