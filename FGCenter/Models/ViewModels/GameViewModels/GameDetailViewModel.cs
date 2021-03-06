﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGCenter.Models.ViewModels
{
    public class GameDetailViewModel
    {

        public ApplicationUser User { get; set; }

        public Post Post { get; set; }

        public Game Game { get; set; }

        public PostWithCommentCountViewModel PostWithComment { get; set; }

        public List<PostWithCommentCountViewModel> PostsWithCommentCount { get; set; }
    }
}
