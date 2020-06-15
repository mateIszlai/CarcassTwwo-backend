using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests
{
    public class MeepleInfo
    {
        public Client Owner { get; private set; }
        public Coordinate Coordinate { get; private set; }
        public int Field { get; private set; }
        public MeepleInfo(Client owner, Coordinate coordinate, int field)
        {
            Owner = owner;
            Coordinate = coordinate;
            Field = field;
        }
    }
}
