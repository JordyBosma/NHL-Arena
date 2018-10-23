using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands;
using Newtonsoft.Json;
using WorldObjects;

namespace Clients
{
    public class ClientReceiveManager
    {

        public ClientReceiveManager()
        {

        }

        public List<Command> ReceiveString(string cmdString)
        {
            try
            {
                dynamic jsonobject = JsonConvert.DeserializeObject(cmdString);
                return ConvertToCommands(jsonobject);
            }
            catch
            {
                List<Command> errorlist = new List<Command>();
                errorlist.Add(new ErrorCommand("fout met dezerializen van json"));
                return errorlist;
            }
        }

        private List<Command> ConvertToCommands(dynamic json)
        {
            try
            {
                List<Command> cmdlist = new List<Command>();
                for (int i = 0; i < json.Count; i++)
                {
                    Random rnd = new Random();
                    switch (json[i].commandType.Value)
                    {
                        case "HitCommand":
                            cmdlist.Add(new HitCommand(new Guid(json[i].shootingPlayerGuid.Value), new Guid(json[i].hitPlayerGuid.Value), (int)json[i].damage.Value));
                            break;
                        case "UpdatePlayerCommand":
                            cmdlist.Add(new UpdatePlayerCommand(new Guid(json[i].playerGuid.Value), 1, 1, 1));
                            break;
                        default:
                            cmdlist.Add(null);
                            break;
                    }
                }
                return cmdlist;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLineIf(e is NullReferenceException, "you called something that doesnt exist.");
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }
    }
}
