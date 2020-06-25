namespace CarcassTwwo.Models.Requests
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        int HandlePlacement(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, int id, bool roadClosed, Coordinate[] surroundingCoords);
        void HandleMeeplePlacement(int placeOfMeeple, Card placedCard, Client owner);
    }
}
