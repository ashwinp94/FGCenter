using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGCenter.Models.ViewModels
{
    public class GameIndexViewModel
    {
        public ApplicationUser User { get; set; }

        public List<GamesWithPostCountViewModel> GamesWithPostCount {get; set;}

        public GamesWithPostCountViewModel Games { get; set; }
    }
}
