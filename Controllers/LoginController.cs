using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ElevkårSida.Models;

namespace ElevkårSida.Controllers
{
    public class LoginController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ElevDBString"].ConnectionString;

        [HttpGet]
        public ActionResult Index()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult Index(UserModel userLogin, string Email, string Password)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                DataTable loginInfo = new DataTable();
                sqlCon.Open();
                string query = "SELECT UserId FROM Tbl_Users WHERE Username = @Username AND Password = @Password";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Username", userLogin.Username);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Password", userLogin.Password);
                sqlDa.Fill(loginInfo);

                if (loginInfo.Rows.Count > 0)
                {
                    Session["LoginId"] = int.Parse(loginInfo.Rows[0]["UserId"].ToString());
                }
                else
                {
                    ViewBag.LoginResult = "Fel lösenord eller epostaddress";
                    return View();
                }
            }

            return RedirectToAction("Index", "Forum");
        }
        public ActionResult Logout()
        {
            Session["LoginId"] = null;
            return RedirectToAction("Index", "Forum");
        }
    }
}