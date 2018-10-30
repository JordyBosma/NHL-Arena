using Commands;
using System;
using System.Collections.Generic;
using System.Timers;
using WorldObjects;

namespace ItemLogic
{
    public class SpawnLogic
    {
        private CommandManager manager;
        private List<SpawnLocation> spawnList = new List<SpawnLocation>();
        
        public SpawnLogic(CommandManager manager)
        {
            this.manager = manager;

            spawnList.Add(new SpawnLocation(28, 1, -35, "DamageBoost"));

            
        }

        public void SpawnTimer()
        {
            foreach (SpawnLocation l in spawnList)
            {
                if (l.hasItem == false && l.hasChanged == true)
                {
                    l.setChanged();
                    SetTimer(l, GenerateInterval(l.itemType));
                }
            }

            Timer spawnTimer = new Timer(1000);
            spawnTimer.Elapsed += (e, v) => {
                SpawnTimer();
                spawnTimer.Dispose();
            };
            spawnTimer.Enabled = true;
        }

        private int GenerateInterval(string itemType)
        {
            switch (itemType)
            {
                case "DamageBoost":
                    //return 50000;
                    return 10000;
                case "SpeedBoost":
                    //return 30000;
                    return 1000;
                case "AHA":
                    //return 30000;
                    return 1000;
            }
            return 1000;
        }

        private void SetTimer(SpawnLocation l, int interval)
        {
            Timer aTimer = new Timer(interval);
            aTimer.Elapsed += (e, v) => {
                OnTimedEvent(l);
                aTimer.Dispose();
            };
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(SpawnLocation l)
        {
            string itemType = "";

            switch (l.itemType)
            {
                case "DamageBoost":
                    Item damageBoost = new Item(l);
                    manager.InitializeItem(damageBoost);
                    break;
                case "SpeedBoost":
                    Item speedBoost = new Item(l);
                    manager.InitializeItem(speedBoost);
                    break;
                case "AHA":
                    Random random = new Random();
                    string[] itemOptions = new string[] { "HealthItem", "HealthItem", "ArmourItem", "AmmoItem", "AmmoItem" };
                    itemType = itemOptions[random.Next(5)];
                    break;
            }

            if (itemType == "HealthItem")
            {
                l.changeItem(itemType);
                HealthItem newHItem = new HealthItem(l);
                manager.InitializeItem(newHItem);
            }
            if (itemType == "ArmourItem")
            {
                l.changeItem(itemType);
                ArmourItem newAItem = new ArmourItem(l);
                manager.InitializeItem(newAItem);
            }
            if (itemType == "AmmoItem")
            {
                l.changeItem(itemType);
                AmmoItem newAmmoItem = new AmmoItem(l);
                manager.InitializeItem(newAmmoItem);
            }
        }
    }
}
