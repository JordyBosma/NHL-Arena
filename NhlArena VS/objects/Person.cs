using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace objects
{
    public class Person
    {
        public int id { get; }
        public string username { get; }

        public Person(int id, string username)
        {
            this.id = id;
            this.username = username;
        }
    }
}
