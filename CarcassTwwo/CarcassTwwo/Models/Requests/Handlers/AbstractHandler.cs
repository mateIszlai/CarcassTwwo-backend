namespace CarcassTwwo.Models.Requests.Handlers
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;
        protected IBoard _board;

        public AbstractHandler(IBoard board)
        {
            _board = board;
        }

        public virtual int HandlePlacement(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, int id, bool roadClosed, Coordinate[] surroundingCoords)
        {
            if(_nextHandler != null)
            {
                _nextHandler.HandlePlacement(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed, surroundingCoords);
            }
            return id;
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
    }
}
