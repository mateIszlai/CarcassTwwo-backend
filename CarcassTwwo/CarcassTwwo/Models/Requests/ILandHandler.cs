using CarcassTwwo.Models.Places;
using System.Collections.Generic;

namespace CarcassTwwo.Models.Requests
{
    public interface ILandHandler
    {
        public HashSet<GrassLand> Lands { get; }
    }
}
