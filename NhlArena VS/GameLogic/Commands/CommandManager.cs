using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLogic;

namespace Commands
{
    public class CommandManager
    {
        private Game game;
        
        public CommandManager(Game game)
        {
            this.game = game;
        }

        public void ReceiveCommand(Command cmd)
        {

        }
    }
}
