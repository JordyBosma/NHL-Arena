﻿using System;
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
            dynamic jsonobject = JsonConvert.DeserializeObject(cmdString);

            return ConvertToCommands(jsonobject);
        }

        private List<Command> ConvertToCommands(dynamic json)
        {
            try
            {
                List<Command> cmdlist = new List<Command>();
                for (int i = 0; i < json.Count; i++)
                {
                    switch (json[i].commandType.Value)
                    {
                        case "HitCommand":
                            cmdlist.Add( new HitCommand(new Player(json[i].shootingPlayer.playerGuid.Value), json[i].hitPlayer.playerGuid.Value, json[i].damage.Value));
                            break;
                        case "ErrorCommand":
                            cmdlist.Add(new ErrorCommand());
                            break;                        
                        default:
                            cmdlist.Add(null);
                            break;
                    }
                }
                return cmdlist;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLineIf(e is NullReferenceException, "you called something that doesnt exist.");
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }
    }
}
