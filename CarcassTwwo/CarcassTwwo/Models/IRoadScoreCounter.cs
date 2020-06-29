using CarcassTwwo.Models.Places;

namespace CarcassTwwo.Models
{
    public interface IRoadScoreCounter
    {
        void CheckOwnerOfRoad(Road road);
    }
}
