using System.Collections.Generic;

namespace CarcassTwwo.Models.Requests
{
    public class CoordinatesWithRotation
    {
        public Coordinate Coordinate { get; set; }
        public List<int> Rotations { get; set; }
    }
}