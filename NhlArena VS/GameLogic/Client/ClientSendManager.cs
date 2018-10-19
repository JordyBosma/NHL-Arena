using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands;

namespace Clients
{
    public class ClientSendManager
    {
        private List<Command> Commandqueue = new List<Command>();

        public ClientSendManager()
        {

        }

        /// <summary>
        /// add command to the queue
        /// </summary>
        /// <param name="cmd">the command to send</param>
        public void AddCommand(Command cmd)
        {
            Commandqueue.Add(cmd);
        }

        /// <summary>
        /// serialized and clears the command queue
        /// </summary>
        /// <returns></returns>
        public string GetCommandsForSending()
        {
            Command[] resultArray = Commandqueue.ToArray();

            Commandqueue.Clear();

            return JsonConvert.SerializeObject(resultArray, Formatting.Indented);
        }
    }
}
