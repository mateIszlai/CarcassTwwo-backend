using System;
using System.Diagnostics.CodeAnalysis;

namespace CarcassTwwo.Models
{
    public class LandType : IEquatable<LandType>
    {
        public int PlaceId { get; set; }
        public string Name { get; set; }
        public MeepleType Meeple { get; set; }

        public bool Equals([AllowNull] LandType other)
        {
            return Name == other.Name;
        }
    }
}
