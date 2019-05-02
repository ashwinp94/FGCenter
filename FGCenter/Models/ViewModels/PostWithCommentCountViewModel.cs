using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGCenter.Models.ViewModels
{
    public class PostWithCommentCountViewModel
    {
        public int NumberOfComments { get;set; }

        public Post Post { get; set; }

    }
}
