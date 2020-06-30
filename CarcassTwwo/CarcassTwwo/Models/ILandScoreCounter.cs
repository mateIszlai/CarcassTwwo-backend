using CarcassTwwo.Models.Places;

namespace CarcassTwwo.Models
{
    public interface ILandScoreCounter
    {
        void CheckOwnerOfLand(GrassLand land, int cities);
    }
}
