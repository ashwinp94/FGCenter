using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FGCenter.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public string Text { get; set; }

        public DateTime DatePosted { get; set; }

        public DateTime EditedDate { get; set; }

        public Post Post { get; set; }

        public int PostId { get; set; }

        public string  UserId { get; set; }

       [Required]
       public ApplicationUser User { get; set; }
    }
}
