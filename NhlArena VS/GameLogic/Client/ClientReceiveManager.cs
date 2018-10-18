using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands;
using Newtonsoft.Json;

namespace Clients
{
    public class ClientReceiveManager
    {
        private CommandManager cmdManager;
        public ClientReceiveManager(CommandManager cmdManager)
        {
            this.cmdManager = cmdManager;
        }

        public dynamic ReceiveString(string cmdString)
        {
            var jsonobject = JsonConvert.DeserializeObject(cmdString);

            return jsonobject;
        }
    }
}
