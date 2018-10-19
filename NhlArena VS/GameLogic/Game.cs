using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Clients;
using WorldObjects;

namespace GameLogic
{
    public class Game
    {
        private bool isActive = false;
        public Guid gameId { get; }
        private List<Player> players = new List<Player>();
        public string gameName { get; }

        public Game(Client initailClient)
        {
            gameId = Guid.NewGuid();
            players.Add(new Player(initailClient));
            gameName = initailClient.username + "'s Game";
            
            Thread gameThread = new Thread(Logic);
            gameThread.Start();
        }

        public bool AddPlayer(Client newClient)
        {
            if(players.Count() < 7)
            {
                players.Add(new Player(newClient));
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
