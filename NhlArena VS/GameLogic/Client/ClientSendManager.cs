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

        public void AddCommand(Command cmd)
        {
            Commandqueue.Add(cmd);
        }

        public string GetCommandsForSending()
        {
            Command[] resultArray = new Command[Commandqueue.Count];

            for(int i = 0; i <Commandqueue.Count; i++)
            {
                resultArray[i] = Commandqueue[i];
            }

            Commandqueue.Clear();

            return JsonConvert.SerializeObject(resultArray);
        }
    }
}
