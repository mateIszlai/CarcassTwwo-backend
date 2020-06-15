using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests
{
    public class MeepleInfo
    {
        Client Owner { get; set; }
        Coordinate Coordinate { get; set; }
        int Field { get; set; }
        public MeepleInfo(Client owner, Coordinate coordinate, int field)
        {
            Owner = owner;
            Coordinate = coordinate;
            Field = field;
        }
    }
}
