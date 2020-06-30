namespace CarcassTwwo.Models
{
    public class Client
    {
        public string Name { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsOwner { get; set; }
        public string ConnectionId { get; set; }
        public int Points { get; set; }
        public int MeepleCount { get; set; }
        public bool Ready { get; set; } = false;

        public Client()
        {
            MeepleCount = 7;
        }
    }
}
