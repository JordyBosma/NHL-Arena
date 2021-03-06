﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using Clients;
using WorldObjects;
using Commands;
using ItemLogic;
using Timer = System.Timers.Timer;

namespace GameLogic
{
    public class Game: IDisposable
    {
        public Guid gameId { get; }
        SpawnManager spawnManager;
        CommandManager commandManager; //handles commands

        Thread gameThread;// thread for the ticktimer
        Timer gameTimer;// timer for time left of the game
        private int timeLeft = 300; //time left of the game in seconds

        private List<Object3D> worldObjects = new List<Object3D>(); //all of the movable world objects
        private bool isActive = true;

        public string gameName { get; }

        /// <summary>
        /// initialise the first game with the initial player
        /// </summary>
        /// <param name="initialClient">the first player</param>
        public Game(Client initialClient)
        {
            gameId = Guid.NewGuid();
            initialClient.SetGameId(gameId);

            commandManager = new CommandManager(this);
            spawnManager = new SpawnManager(commandManager);

            PlayerSpawnLocation spawn = spawnManager.GetPlayerSpawnLocation();
            Player initialPlayer = new Player(initialClient, spawn.x, spawn.y, spawn.z, spawn.rotationX, spawn.rotationY, spawn.rotationZ);
            worldObjects.Add(initialPlayer);
            gameName = initialClient.username + "'s Game";

            

            //subscribes commandmanager and client to each other
            initialClient.Subscribe(commandManager);
            commandManager.Subscribe(initialClient);

            commandManager.InitializePlayer(initialPlayer);

            gameThread = new Thread(TickTimer);
            gameThread.Start();

            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.AutoReset = true;
            gameTimer.Elapsed += (v, e) => GameTimerElapsed();
            gameTimer.Start();
        }

        /// <summary>
        /// adds a new player to the game
        /// </summary>
        /// <param name="newClient">the new player</param>
        /// <returns></returns>
        public bool AddPlayer(Client newClient)
        {
            if (GetPlayerCount() < 7)
            {
                PlayerSpawnLocation spawn = spawnManager.GetPlayerSpawnLocation();
                Player newPlayer = new Player(newClient, spawn.x, spawn.y, spawn.z, spawn.rotationX, spawn.rotationY, spawn.rotationZ);
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

        /// <summary>
        /// the timer that triggers the sending of commands to the clients
        /// </summary>
        public void TickTimer()
        { 
            while (isActive)
            {
                Thread.Sleep(1000 / 60);
                commandManager.SendCommandQueue();
            }
        }

        /// <summary>
        /// the game timer that updates the time left for connected clients and triggers the dispose of the game
        /// </summary>
        public void GameTimerElapsed()
        {
            timeLeft--;
            if (timeLeft >= 0) { 
                GameTimeLeftCommand cmd = new GameTimeLeftCommand(timeLeft);
                commandManager.SendGameTimeLeftCommand(cmd);
            }
            if (timeLeft == 0)
            {
                GameEndingCommand cmd = new GameEndingCommand();
                commandManager.SendGameEndingCommand(cmd);
            }
            if (timeLeft == -25)
            {
                this.Dispose();
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

        public int GetGameTimeLeft()
        {
            return 200;
		}
		
        /// <summary>
        /// stops all timers and async threads
        /// </summary>
        public void Dispose()
        {
            gameTimer.Dispose();
            commandManager.DisconnectAllClients();
            isActive = false;
            spawnManager.Dispose();

            //DEZE ALS ALLERLAATST!!!
            GameManager.RemoveGame(this);
        }
    }
}
