using CarcassTwwo.Models.Places;

namespace CarcassTwwo.Models.Requests.Handlers
{
    public interface ICityCounter
    {
        int GetFinishedCitiesOfLand(GrassLand land);
    }
}
