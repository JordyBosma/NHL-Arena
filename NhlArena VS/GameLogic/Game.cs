using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Clients;

namespace GameLogic
{
    public class Game
    {
        private bool isActive = false;
        public Guid gameId { get; }
        List<Client> players = new List<Client>();

        public Game(Client initailPlayer)
        {
            gameId = new Guid();
            players.Add(initailPlayer);

            Thread gameThread = new Thread(Logic);
            gameThread.Start();
        }

        public bool AddPlayer(Client newPlayer)
        {
            if(players.Count() < 7)
            {
                players.Add(newPlayer);
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
    }
}
