namespace CarcassTwwo.Models.Requests
{
    public interface ICityAdder
    {
        void AddCityToLand(Card card, int landCounts, int cityId);
        void ChangeCityIdInLand(int landId, int cityIdToChange, int newCityId);
    }
}
