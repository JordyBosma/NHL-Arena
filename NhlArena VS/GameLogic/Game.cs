using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Clients;
using WorldObjects;
using Commands;

namespace GameLogic
{
    public class Game
    {
        private bool isActive = false;
        public Guid gameId { get; }
        CommandManager commandManager;
        
        private List<Player> players = new List<Player>();
        public string gameName { get; }

        public Game(Client initialClient)
        {
            gameId = Guid.NewGuid();
            players.Add(new Player(initialClient, 0, 0, 0, 0, 0, 0));
            gameName = initialClient.username + "'s Game";

            commandManager = new CommandManager(this);

            //subscribes commandmanager and client to each other
            initialClient.Subscribe(commandManager);
            commandManager.Subscribe(initialClient);

            Thread gameThread = new Thread(Logic);
            gameThread.Start();
        }

        public bool AddPlayer(Client newClient)
        {
            if(players.Count() < 7)
            {
                players.Add(new Player(newClient, 0, 0, 0, 0, 0, 0));

                //subscribes commandmanager and client to each other
                newClient.Subscribe(commandManager);
                commandManager.Subscribe(newClient);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Logic()
        {
            isActive = true;

            while (isActive)
            {
                Thread.Sleep(1000 / 60);
            }
        }

        public int GetPlayerCount()
        {
            return players.Count();
        }
    }
}
