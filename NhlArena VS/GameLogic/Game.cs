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

        public Game(Client initailPlayer)
        {
            gameId = new Guid();
            players.Add(new Player(initailPlayer));
            gameName = initailPlayer.username + "'s Game";
            
            Thread gameThread = new Thread(Logic);
            gameThread.Start();
        }

        public bool AddPlayer(Client newPlayer)
        {
            if(players.Count() < 7)
            {
                players.Add(new Player(newPlayer));
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
