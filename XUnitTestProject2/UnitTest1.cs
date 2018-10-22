using System;
using System.IO;
using Xunit;
using Clients;
using Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorldObjects;

namespace XUnitTestProject2
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            ClientReceiveManager crm = new ClientReceiveManager();

            List<Command> cmdlist = new List<Command>();

            Player p = new Player(null, 0, 0, 0, 0, 0, 0);
            Player x = new Player(null, 0, 0, 0, 0, 0, 0);


            HitCommand cmd = new HitCommand(p.playerGuid, x.playerGuid, 10);
            HitCommand cmd2 = new HitCommand(x.playerGuid, p.playerGuid, 15);
            cmdlist.Add(cmd);
            cmdlist.Add(cmd2);

            string jsonstring = JsonConvert.SerializeObject(cmdlist, Formatting.Indented);


            List<Command> result = crm.ReceiveString(jsonstring);
            for (int i = 0; i < cmdlist.Count; i++)
            {
                Assert.Equal(cmdlist[i].ToString(), result[i].ToString());
            }

        }
    }
}
