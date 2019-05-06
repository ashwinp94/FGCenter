using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGCenter.Models.ViewModels
{
    public class PostDetailViewModel
    {

        public List<Comment> GroupedComments { get; set; }

        public Comment Comment { get; set; }

        public Post Post { get; set; }

        public Game Game { get; set; }

        public ApplicationUser User { get; set; }
    }
}
