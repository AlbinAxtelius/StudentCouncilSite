using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElevkårSida.Models;


namespace ElevkårSida.Controllers
{
    public class ForumController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ElevDBString"].ConnectionString;

        public ActionResult Index()
        {
            ForumModel forumModel = new ForumModel();
            DataTable posts = new DataTable();
            List<PostModel> PostModel = new List<PostModel>();


            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string getPosts = "SELECT * FROM Tbl_Posts ORDER BY Tbl_Posts.PostId DESC";
                SqlDataAdapter sqlDa = new SqlDataAdapter(getPosts, sqlCon);
                sqlDa.Fill(posts);


                for (int i = 0; i < posts.Rows.Count; i++)
                {
                    PostModel.Add(new Models.PostModel
                    {
                        PostId = int.Parse(posts.Rows[i]["PostId"].ToString()),
                        PostContent = posts.Rows[i]["PostContent"].ToString(),
                        PostFile = posts.Rows[i]["PostFile"].ToString(),
                        Date = posts.Rows[i]["Date"].ToString(),
                    });
                }

                forumModel.PostModels = PostModel;
            }
            return View(forumModel);
        }

        [HttpPost]
        public ActionResult CreatePost(PostModel postModel, HttpPostedFileBase postFile)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Tbl_Posts(PostContent, Postfile, Date) VALUES (@PostContent,@Postfile,@Date)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@PostContent", postModel.PostContent);
                sqlCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

                if (postFile == null)
                {
                    sqlCmd.Parameters.AddWithValue("@PostFile", DBNull.Value);
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@PostFile", "Content/PostedImages/" + postFile.FileName);

                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/PostedImages/"), fileName);
                            file.SaveAs(path);
                        }
                    }
                }
                sqlCmd.ExecuteReader();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Feedback()
        {
            return View(new FeedbackModel());
        }
        [HttpPost]
        public ActionResult Feedback(FeedbackModel feedbackModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Tbl_Feedback(Title,FeedbackText,Author,Archived) VALUES (@Title,@FeedbackText,@Author, 0)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Title", feedbackModel.Title);
                sqlCmd.Parameters.AddWithValue("@FeedbackText", feedbackModel.FeedbackText);

                if (Request.Form["Hidden"] != "on")
                {
                    sqlCmd.Parameters.AddWithValue("@Author", feedbackModel.Author);
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@Author", DBNull.Value);
                }
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Forum");
        }
        [HttpPost]
        public ActionResult Like(int id, PostModel postModel)
        {
            string query = "";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                if (postModel.Liked == 1)
                {
                    query = "DELETE FROM Tbl_Likes WHERE UserId = @UserId AND PostId = @PostId";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@UserId", Session["LoginId"]);
                    sqlCmd.Parameters.AddWithValue("@PostId", id);
                    sqlCmd.ExecuteNonQuery();
                }
                else
                {
                    query = "INSERT INTO Tbl_Likes(UserId, PostId) VALUES(@UserId ,@PostId)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@UserId", Session["LoginId"]);
                    sqlCmd.Parameters.AddWithValue("@PostId", id);
                    sqlCmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index", "Forum");
        }

        public ActionResult Delete(int id)
        {
            if (Session["LoginId"] != null)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM Tbl_Posts WHERE PostId = @PostId";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@PostId", id);
                    sqlCmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index", "Forum");
        }
    }
}