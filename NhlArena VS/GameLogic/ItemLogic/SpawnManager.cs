using Commands;
using System;
using System.Collections.Generic;
using System.Timers;
using WorldObjects;

namespace ItemLogic
{
    public class SpawnManager : IDisposable
    {
        private CommandManager cmdManager;
        private List<SpawnLocation> spawnList = new List<SpawnLocation>();
        Timer spawnTimer;
        List<Timer> spawnLocationTimers = new List<Timer>();

        public SpawnManager(CommandManager manager)
        {
            this.cmdManager = manager;

            spawnList.Add(new SpawnLocation(28, 1, -35, "DamageBoost"));

            spawnTimer = new Timer();
            spawnTimer.Interval = 1000;
            spawnTimer.Elapsed += (e, v) => {
                SpawnTimer();
            };
            spawnTimer.Start();
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
                spawnLocationTimers.Remove(aTimer);
                aTimer.Dispose();
            };
            aTimer.Enabled = true;
            spawnLocationTimers.Add(aTimer);
        }

        private void OnTimedEvent(SpawnLocation l)
        {
            string itemType = "";

            switch (l.itemType)
            {
                case "DamageBoost":
                    Item damageBoost = new Item(l);
                    cmdManager.InitializeItem(damageBoost);
                    break;
                case "SpeedBoost":
                    Item speedBoost = new Item(l);
                    cmdManager.InitializeItem(speedBoost);
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
                cmdManager.InitializeItem(newHItem);
            }
            if (itemType == "ArmourItem")
            {
                l.changeItem(itemType);
                ArmourItem newAItem = new ArmourItem(l);
                cmdManager.InitializeItem(newAItem);
            }
            if (itemType == "AmmoItem")
            {
                l.changeItem(itemType);
                AmmoItem newAmmoItem = new AmmoItem(l);
                cmdManager.InitializeItem(newAmmoItem);
            }
        }

        public void Dispose()
        {
            spawnTimer.Dispose();

            foreach (Timer t in spawnLocationTimers)
            {
                t.Dispose();
            }
        }
    }
}
