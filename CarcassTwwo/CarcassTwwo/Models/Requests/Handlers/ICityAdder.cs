namespace CarcassTwwo.Models.Requests.Handlers
{
    public interface ICityAdder
    {
        void AddCityToLand(Card card, int landCounts, int cityId);
        void ChangeCityIdInLand(int landId, int cityIdToChange, int newCityId);

        void AddCityToLand(int landId, int cityId);
    }
}
