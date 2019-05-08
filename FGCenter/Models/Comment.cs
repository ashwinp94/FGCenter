using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FGCenter.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public string Text { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatePosted { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EditedDate { get; set; }

        public Post Post { get; set; }

        [ForeignKey("Posts")]
        public int PostId { get; set; }

        public string  UserId { get; set; }

       [Required]
       public ApplicationUser User { get; set; }
    }
}
