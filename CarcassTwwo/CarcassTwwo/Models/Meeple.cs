using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Meeple
    {
        public MeepleType Type { get; set; }
        public Client Owner { get; set; }
        public Meeple( MeepleType type, Client owner)
        {
            Type = type;
            Owner = owner;
        }
    }
}
