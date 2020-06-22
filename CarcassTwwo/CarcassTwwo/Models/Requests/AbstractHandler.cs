namespace CarcassTwwo.Models.Requests
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public virtual int Handle(Card topCard, Card botCard, Card leftCard, Card rightCard, Card card, int landCounts, int id, bool roadClosed)
        {
            if(_nextHandler != null)
            {
                _nextHandler.Handle(topCard, botCard, leftCard, rightCard, card, landCounts, id, roadClosed);
            }
            return id;
        }

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }
    }
}
