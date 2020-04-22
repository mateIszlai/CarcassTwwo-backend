using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Models
{
    public class Client
    {
        public string Name { get; set; }
        [NotMapped]
        public List<Client> CoPlayers { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsWaitingForMove { get; set; }
        public string ConnectionId { get; set; }
        public int Points { get; set; }
    }
}
