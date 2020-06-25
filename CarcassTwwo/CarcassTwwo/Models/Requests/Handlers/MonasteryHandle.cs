using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests.Handlers
{
    public class MonasteryHandle : AbstractHandler
    {
        public HashSet<Monastery> Monasteries { get; private set; }
        
    }
}
