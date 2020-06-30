namespace CarcassTwwo.Models.Places
{
    public class CityPart
    {
        public int CardId { get; set; }
        public bool TopIsOpen { get; set; }
        public bool LeftIsOpen { get; set; }
        public bool BottomIsOpen { get; set; }
        public bool RightIsOpen { get; set; }
        public bool HasCrest { get; set; }

        public CityPart(int cardId)
        {
            CardId = cardId;
            LeftIsOpen = BottomIsOpen = RightIsOpen = TopIsOpen = true;
        }
    }
}
