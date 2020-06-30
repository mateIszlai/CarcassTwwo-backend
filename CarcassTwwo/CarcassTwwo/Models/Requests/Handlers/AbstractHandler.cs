using System.Collections.Generic;

namespace CarcassTwwo.Models.Requests.Handlers
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;
        protected static int id = 0;
        protected IBoard _board;

        public AbstractHandler(IBoard board)
        {
            _board = board;
        }

        public virtual object HandlePlacement(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, bool roadClosed, Coordinate[] surroundingCoords)
        {
            if(_nextHandler != null)
            {
                _nextHandler.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, roadClosed, surroundingCoords);
            }
            return null;
        }

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void HandleMeeplePlacement(int placeOfMeeple, Card placedCard, Client owner)
        {
            if (_nextHandler != null)
                _nextHandler.HandleMeeplePlacement(placeOfMeeple, placedCard, owner);
        }

        public virtual void HandleScore(List<Meeple> meeples)
        {
            if (_nextHandler != null)
                _nextHandler.HandleScore(meeples);
            else
                return;
        }

        public virtual void HandleEndScore()
        {
            if (_nextHandler != null)
                _nextHandler.HandleEndScore();
        }
    }
}
