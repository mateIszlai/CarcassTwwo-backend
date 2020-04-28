using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Group
    {
        public HashSet<Client> Members { get; set; }
        public Game Game { get; set; }
        public Group()
        {
            Members = new HashSet<Client>();
        }
    }
}
