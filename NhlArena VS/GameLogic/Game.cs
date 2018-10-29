using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Clients;
using WorldObjects;
using Commands;
using ItemLogic;

namespace GameLogic
{
    public class Game
    {
        private bool isActive = false;
        public Guid gameId { get; }
        CommandManager commandManager;
        SpawnLogic spawnLogic;

        private List<Object3D> worldObjects = new List<Object3D>();
        public string gameName { get; }

        public Game(Client initialClient)
        {
            gameId = Guid.NewGuid();
            initialClient.SetGameId(gameId);
            Player initialPlayer = new Player(initialClient, 0, 0, 0, 0, 0, 0);
            worldObjects.Add(initialPlayer);
            gameName = initialClient.username + "'s Game";

            commandManager = new CommandManager(this);
            spawnLogic = new SpawnLogic(commandManager);

            //subscribes commandmanager and client to each other
            initialClient.Subscribe(commandManager);
            commandManager.Subscribe(initialClient);

            commandManager.InitializePlayer(initialPlayer);

            Thread gameThread = new Thread(TickTimer);
            gameThread.Start();
        }

        public bool AddPlayer(Client newClient)
        {
            if (worldObjects.Count() < 7)
            {
                Player newPlayer = new Player(newClient, 0, 0, 0, 0, 0, 0);
                worldObjects.Add(newPlayer);

                //subscribes commandmanager and client to each other
                newClient.Subscribe(commandManager);
                commandManager.Subscribe(newClient);
                commandManager.InitializePlayer(newPlayer);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TickTimer()
        {
            isActive = true;


            while (isActive)
            {
                Thread.Sleep(1000 / 60);
                commandManager.SendCommandQueue();
            }
        }

        public List<Object3D> getWorldObjects()
        {
            return worldObjects;
        }

        public int GetPlayerCount()
        {
            int count = 0;

            foreach (Object3D obj in worldObjects)
            {
                if (obj is Player)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
