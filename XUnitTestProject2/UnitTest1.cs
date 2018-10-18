using System;
using Xunit;
using Clients;
using Commands;

namespace XUnitTestProject2
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            CommandManager cmdm = new CommandManager(null);
            ClientReceiveManager crm = new ClientReceiveManager(cmdm);

            var result = crm.ReceiveString("[ { \"Naam\": \"JSON\", \"Type\": \"Gegevensuitwisselingsformaat\", \"isProgrammeertaal\": false, \"Zie ook\": [\"XML\", \"ASN.1\"] }, { \"Naam\": \"JavaScript\", \"Type\": \"Programmeertaal\", \"isProgrammeertaal\": true, \"Jaar\": 1995   } ]");
        }
}
}
