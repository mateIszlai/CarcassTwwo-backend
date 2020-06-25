namespace CarcassTwwo.Models.Requests
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        int Handle(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, int id, bool roadClosed, Coordinate[] surroundingCoords);
    }
}
