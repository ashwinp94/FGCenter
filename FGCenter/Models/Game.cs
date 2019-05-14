using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGCenter.Models
{
    public class Game
    {
        public int GameId { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string ShortenedName { get; set; }

        public string DeveloperName { get; set; }
    }
}
