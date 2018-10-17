using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Command
    {
        public string commandType { get; }

        public Command(string commandType)
        {
            this.commandType = commandType;
        }
    }
}
