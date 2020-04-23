using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class LocalGroup
    {
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public List<Client> Members { get; set; }
    }
}
