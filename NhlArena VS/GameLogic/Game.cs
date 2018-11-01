using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using Clients;
using WorldObjects;
using Commands;
using Timer = System.Timers.Timer;

namespace GameLogic
{
    public class Game: IDisposable
    {
        public Guid gameId { get; }
        CommandManager commandManager; //handles commands

        Thread gameThread;// thread for the ticktimer
        Timer gameTimer;// timer for time left of the game
        private int timeLeft = 60; //time left of the game in seconds

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
            if (timeLeft == -300)
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
            //dispose spawnmanager hier !!!!!


            //DEZE ALS ALLERLAATST!!!
            GameManager.RemoveGame(this);
        }
    }
}
