﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Clients;
using WorldObjects;
using Commands;

namespace GameLogic
{
    public class Game: IDisposable
    {
        public Guid gameId { get; }
        CommandManager commandManager; //handles commands

        Thread gameThread;// thread for the ticktimer

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
            Player initialPlayer = new Player(initialClient, 0, 0, 0, 0, 0, 0);
            worldObjects.Add(initialPlayer);
            gameName = initialClient.username + "'s Game";

            commandManager = new CommandManager(this);

            //subscribes commandmanager and client to each other
            initialClient.Subscribe(commandManager);
            commandManager.Subscribe(initialClient);

            commandManager.InitializePlayer(initialPlayer);

            gameThread = new Thread(TickTimer);
            gameThread.Start();
        }

        /// <summary>
        /// adds a new player to the game
        /// </summary>
        /// <param name="newClient">the new player</param>
        /// <returns></returns>
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

        /// <summary>
        /// stops all timers and async threads
        /// </summary>
        public void Dispose()
        {
            commandManager.DisconnectAllClients();
            isActive = false;
            //dispose spawnmanager hier !!!!!


            //DEZE ALS ALLERLAATST!!!
            GameManager.RemoveGame(this);
        }
    }
}
