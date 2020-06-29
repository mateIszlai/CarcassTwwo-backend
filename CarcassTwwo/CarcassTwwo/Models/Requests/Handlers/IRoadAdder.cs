using CarcassTwwo.Models.Places;

namespace CarcassTwwo.Models.Requests.Handlers
{
    public interface IRoadAdder
    {
        void AddRoadToLand(Side side, int tempId, Card card);
        void ChangeRoadIdInLand(int landId, int roadIdToChange, int newRoadId);
    }
}
