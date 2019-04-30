using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGCenter.Models.ViewModels
{
    public class GameDetailViewModel
    {
        public List<Post> GroupedPosts { get; set; }

        public Post Post { get; set; }

        public Game Game { get; set; }


    }
}
