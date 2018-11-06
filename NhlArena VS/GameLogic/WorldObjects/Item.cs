using ItemLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldObjects
{
    public class Item : Object3D
    {
        public Item(SpawnLocation l) :base(l.x, l.y, l.z, 0, 0, 0, l.itemType)
        {

        }
    }

    public class AmmoItem : Item
    {
        public int weaponId { get; }
        public int itemValue { get; }

        public AmmoItem(SpawnLocation l) : base(l)
        {
            Random random = new Random();
            weaponId = random.Next(3);

            if (weaponId == 0)
            {
                this.itemValue = 100;
            }

            if (weaponId == 1)
            {
                this.itemValue = 15;
            }

            if (weaponId == 2)
            {                
                this.itemValue = 50;
            }
        }
    }

    public class HealthItem : Item
    {
        public int itemValue { get; }

        public HealthItem(SpawnLocation l) : base(l)
        {
            Random random = new Random();
            int[] itemOptions = new int[] { 20, 20, 20, 30, 30, 40 };
            this.itemValue = itemOptions[random.Next(6)];
        }
    }

    public class ArmourItem : Item
    {
        public int itemValue { get; }

        public ArmourItem(SpawnLocation l) : base(l)
        {
            Random random = new Random();
            int[] itemOptions = new int[] { 20, 20, 20, 40, 40, 50 };
            this.itemValue = itemOptions[random.Next(6)];
        }
    }
}
