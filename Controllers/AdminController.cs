using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElevkårSida.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ElevkårSida.Controllers
{
    public class AdminController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ElevDBString"].ConnectionString;

        public ActionResult Index()
        {
            if (Session["LoginId"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Forum");
        }
        public ActionResult Feedback()
        {
            if (Session["LoginId"] != null)
            {
                List<FeedbackModel> feedbackModels = new List<FeedbackModel>();
                AdminModel adminFeedbackModel = new AdminModel();

                DataTable results = new DataTable();

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "SELECT * FROM Tbl_Feedback WHERE Tbl_Feedback.Archived = 0 ORDER BY Tbl_Feedback.FeedbackId DESC";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                    sqlDa.Fill(results);

                    if (results.Rows.Count > 0)
                    {
                        for (int i = 0; i < results.Rows.Count; i++)
                        {
                            feedbackModels.Add(new FeedbackModel
                            {
                                FeedbackId = Convert.ToInt32((results.Rows[i]["FeedbackId"].ToString())),
                                Title = results.Rows[i]["Title"].ToString(),
                                FeedbackText = results.Rows[i]["FeedbackText"].ToString(),
                                Author = results.Rows[i]["Author"].ToString()
                            });
                        }
                        adminFeedbackModel.FeedbackModels = feedbackModels;
                    }
                }
                return View(adminFeedbackModel);
            }
            return RedirectToAction("Index", "Forum");
        }

        public ActionResult Admins()
        {
            if (Session["LoginId"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Forum");
        }
        public ActionResult Archive(int id)
        {
            if (Session["LoginId"] != null)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE Tbl_Feedback SET Archived = 1 WHERE FeedbackId = @FeedbackId";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@FeedbackId", id);
                    sqlCmd.ExecuteNonQuery();

                    return RedirectToAction("Feedback", "Admin");
                }
            }
            return RedirectToAction("Index", "Forum");
        }
        [HttpPost]
        public JsonResult getAdmins()
        {
            DataTable Users = new DataTable();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Tbl_Users";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.Fill(Users);

            }

            JsonSerializer serializer = new JsonSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in Users.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in Users.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            string json = JsonConvert.SerializeObject(Users, Formatting.Indented);

            return new JsonResult { Data = json };
        }
        [HttpPost]
        public JsonResult AddAdmin(string Username, string Password)
        {
            string result = "";

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                DataTable Admins = new DataTable();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Tbl_Users WHERE Username = @Username", sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Username", Username);
                sqlDa.Fill(Admins);
                result = "{\"Result\": \"Användarnamn taget\", \"Color\":\"#e74c3c\"}";

                if (Admins.Rows.Count == 0)
                {
                    SqlCommand sqlCmd = new SqlCommand("INSERT INTO Tbl_Users(Username,Password) VALUES (@Username, @Password)", sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Username", Username);
                    sqlCmd.Parameters.AddWithValue("@Password", Password);
                    sqlCmd.ExecuteNonQuery();
                    result = "{\"Result\": \"Admin tillagd\", \"Color\": \"#2ecc71\"}";
                }
            }
            return new JsonResult { Data = result };
        }
        [HttpPost]
        public JsonResult RemoveAdmin(int UserId)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("DELETE FROM Tbl_Users WHERE UserId = @UserId", sqlCon);
                sqlCmd.Parameters.AddWithValue("@UserId", UserId);
                sqlCmd.ExecuteNonQuery();
            }

            return new JsonResult { Data = "" };
        }
    }
}