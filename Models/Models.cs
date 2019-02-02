using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElevkårSida.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Användarnamn saknas")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Lösenord saknas")]
        public string Password { get; set; }
    }
    public class PostModel
    {
        public int PostId { get; set; }
        public string PostContent { get; set; }
        public string PostFile { get; set; }
        public string Date { get; set; }
        public int Liked { get; set; }
        public int TotalLikes { get; set; }
    }
    public class FeedbackModel
    {
        public int FeedbackId { get; set; }

        [Required(ErrorMessage = "Måste ha en titel")]
        [MaxLength(40, ErrorMessage = "För lång titel (max 40 bokstäver)")]
        public string Title { get; set; }

        [MaxLength(250)]
        [Required(ErrorMessage = "Måste vara ifylld")]
        public string FeedbackText { get; set; }
        [Required(ErrorMessage = "Måste ha ett namn. Fyll i ovan om du vill vara anonym")]
        [MaxLength(70)]

        public string Author { get; set; }
    }
    public class MonitorModel
    {
        public List<PostModel> PostModels { get; set; }
    }
}

