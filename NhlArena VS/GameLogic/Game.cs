using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Views;

namespace NhlArena_VS.GameLogic
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

        public void AddPlayer(Client newPlayer)
        {
            if(players.Count() != 6)
            {
                players.Add(newPlayer);
            }
            else
            {
                //playermax reached
            }
        }

        public void Logic()
        {
            isActive = true;

            while (isActive)
            {
                Thread.Sleep(25);               
            }
        }
    }
}
