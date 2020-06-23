using CarcassTwwo.Models.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models.Requests
{
    public class LandHandler : AbstractHandler, ILandHandler
    {
        public HashSet<GrassLand> Lands { get; set; }
    }
}
