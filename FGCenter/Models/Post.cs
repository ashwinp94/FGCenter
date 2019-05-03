using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGCenter.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public string Title { get; set; }


        public string Text { get; set; }

        public DateTime DatePosted { get; set; }

        public DateTime EditedDate { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        
        public Game Game { get; set; }

        [ForeignKey("Games")]
        public int GameId { get; set; }
    }
}
