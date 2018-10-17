﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLogic
{
    /// <summary>
    /// this class manages all the active games.
    /// </summary>
    public static class GameManager
    {
        static List<Game> activeGames = new List<Game>();

        /// <summary>
        /// checks if the client wants to connect to a new game or a excisting game, 
        /// then gives the client to the corrosponding game.
        /// </summary>
        /// <param name="cs"></param>
        public static void ManageClient(Client cs)
        {
            if (cs.gameId != null)
            {
                foreach (Game game in activeGames)
                {
                    if (game.gameId == cs.gameId)
                    {
                        if (game.AddPlayer(cs))
                        {
                            goto Outer;
                        }
                        else
                        {
                            //give user imput why the socket will be closed, error game is full
                            cs.OnError();
                            cs = null;
                            goto Outer;
                        }
                    }
                }
            }
            else
            {
                if (CreateGame(cs))
                {
                    goto Outer;
                }
                else
                {
                    //give user imput why the socket will be closed, error gamemax reached
                    cs.OnError();
                    cs = null;
                    goto Outer;
                }
            }
        Outer:
            return;
        }

        /// <summary>
        /// checks if gamemax of three async running games is reached,
        /// if not creates a new game.
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        private static bool CreateGame(Client cs)
        {
            if(activeGames.Count < 4)
            {
                activeGames.Add(new Game(cs));
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}