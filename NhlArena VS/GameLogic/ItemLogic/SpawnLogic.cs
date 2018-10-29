using Commands;
using System;
using System.Collections.Generic;
using System.Timers;

namespace ItemLogic
{
    public class SpawnLogic
    {
        private CommandManager manager;
        private List<SpawnLocation> BoostList = new List<SpawnLocation>();
        private List<SpawnLocation> AMAList = new List<SpawnLocation>();
        
        public SpawnLogic(CommandManager manager)
        {
            this.manager = manager;

            BoostList.Add(new SpawnLocation(28, 1, -35, "UltBoost"));
        }

        private void SpawnTimer()
        {
            foreach (SpawnLocation l in BoostList)
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
                case "UltBoost":
                    break;
                case "Boost":
                    break;
                case "AMA":
                    break;
            }
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
            
            
        }
    }
}
