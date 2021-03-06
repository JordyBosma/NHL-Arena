﻿using Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using WorldObjects;

namespace ItemLogic
{
    public class SpawnManager : IDisposable
    {
        private bool initialSpawn = true;
        private int initalSpawnCount = 0;
        private CommandManager cmdManager;
        private List<SpawnLocation> spawnList = new List<SpawnLocation>();
        private PlayerSpawnLocationList playerSpawnList = new PlayerSpawnLocationList();
        private Timer spawnTimer;
        private List<Timer> spawnLocationTimers = new List<Timer>();
        private List<Timer> removedLocationTimers = new List<Timer>();

        public SpawnManager(CommandManager manager)
        {
            this.cmdManager = manager;

            spawnList.Add(new SpawnLocation(26.4, -3.5, -37, "DamageBoost"));
            spawnList.Add(new SpawnLocation(-32.4, 10, -82, "AHA"));
            spawnList.Add(new SpawnLocation(51.8, 0, -57.5, "AHA"));
            spawnList.Add(new SpawnLocation(-13.4, 10, -7.1, "SpeedBoost"));
            spawnList.Add(new SpawnLocation(-31.4, 0, 27.9, "SpeedBoost"));
            spawnList.Add(new SpawnLocation(20, 0, 36, "AHA"));
            spawnList.Add(new SpawnLocation(-46.4, 0, 57, "AHA"));
            spawnList.Add(new SpawnLocation(-51, 0, -13.4, "AHA"));
            spawnList.Add(new SpawnLocation(-24, 0, -28, "AHA"));
            spawnList.Add(new SpawnLocation( 64, 10, -8, "AHA"));
            spawnList.Add(new SpawnLocation(3.2, 0, 29.5, "AHA"));
            spawnList.Add(new SpawnLocation(-3, 0, -66, "AHA"));
            spawnList.Add(new SpawnLocation(-9, 0, 0.5, "AHA"));

            playerSpawnList.AddLocation(-37, 2, 9.55, -1.60);
            playerSpawnList.AddLocation(-10.15, 2, 50.95, 0.00);
            playerSpawnList.AddLocation(-46, 2, -47.25, -1.50);
            playerSpawnList.AddLocation(31.75, 12, -11.30, -6.25);
            playerSpawnList.AddLocation(30, 12, -10.75, -6.25);
            playerSpawnList.AddLocation(30, 12, -76, 1.70);
            cmdManager.InitializeSpawnList(spawnList, playerSpawnList);

            spawnTimer = new Timer();
            spawnTimer.Interval = 1000;
            spawnTimer.Elapsed += (e, v) => {
                SpawnTimer();
            };
            spawnTimer.Start();
        }

        public async void SpawnTimer()
        {
            foreach (SpawnLocation l in spawnList)
            {
                await Task.Delay(50);
                if (l.item == null && l.hasChanged == true)
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
                    return 50000;
                case "SpeedBoost":
                    return 30000;
                case "AHA":
                    if(initialSpawn == true)
                    {
                        initalSpawnCount++;
                        if (initalSpawnCount == 10)
                        {
                            initialSpawn = false;
                        }
                        return 5000;
                    }
                    return 15000;
            }
            return 1000;
        }

        private void SetTimer(SpawnLocation l, int interval)
        {
            Timer aTimer = new Timer(interval);
            aTimer.Elapsed += (e, v) => 
            {
                OnTimedEvent(l);
                spawnLocationTimers.Remove(aTimer);
                //aTimer.Dispose();
            };
            aTimer.AutoReset = false;
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
                    l.addItem(damageBoost);
                    cmdManager.InitializeItem(damageBoost);
                    break;
                case "SpeedBoost":
                    Item speedBoost = new Item(l);
                    l.addItem(speedBoost);
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
                l.addItem(newHItem);
                cmdManager.InitializeItem(newHItem);
                l.changeItem("AHA");
            }
            if (itemType == "ArmourItem")
            {
                l.changeItem(itemType);
                ArmourItem newAItem = new ArmourItem(l);
                l.addItem(newAItem);
                cmdManager.InitializeItem(newAItem);
                l.changeItem("AHA");
            }
            if (itemType == "AmmoItem")
            {
                l.changeItem(itemType);
                AmmoItem newAmmoItem = new AmmoItem(l);
                l.addItem(newAmmoItem);
                cmdManager.InitializeItem(newAmmoItem);
                l.changeItem("AHA");
            }
        }

        public PlayerSpawnLocation GetPlayerSpawnLocation()
        {
            return playerSpawnList.GetSpawnLocation();
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
