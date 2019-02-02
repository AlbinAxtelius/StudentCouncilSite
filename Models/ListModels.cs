using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElevkårSida.Models
{
    public class ForumModel
    {
        public List<PostModel> PostModels { get; set; }
    }
    public class AdminModel
    {
        public List<FeedbackModel> FeedbackModels { get; set; }
        public List<UserModel> userModels { get; set; }
    }
}