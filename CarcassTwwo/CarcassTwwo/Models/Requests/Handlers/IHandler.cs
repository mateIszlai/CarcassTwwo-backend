using System.Collections.Generic;

namespace CarcassTwwo.Models.Requests
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object HandlePlacement(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, bool roadClosed, Coordinate[] surroundingCoords);
        void HandleMeeplePlacement(int placeOfMeeple, Card placedCard, Client owner);
        void HandleScore(List<Meeple> meeples);
        void HandleEndScore();
    }
}
