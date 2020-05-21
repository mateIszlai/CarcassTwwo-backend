using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Area
    {
        LandType _landType { get; }
        List<Field> _fields { get; }

        public Area(LandType landType)
        {
            _landType = landType;
            _fields = new List<Field>();
        }

        public void AddField(Field field)
        {
            _fields.Add(field);
        }
    }
}
