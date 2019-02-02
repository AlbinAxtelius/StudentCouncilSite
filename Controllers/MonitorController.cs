using ElevkårSida.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevkårSida.Controllers
{
    public class MonitorController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ElevDBString"].ConnectionString;
        public ActionResult Index()
        {
            DataTable daTbl = new DataTable();
            MonitorModel monitorModel = new MonitorModel();
            List<PostModel> postModels = new List<PostModel>();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 3 * FROM Tbl_Posts ORDER BY PostId desc";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.Fill(daTbl);

                if (daTbl.Rows.Count > 0)
                {
                    for (int i = 0; i < daTbl.Rows.Count; i++)
                    {
                        postModels.Add(new PostModel
                        {
                            PostContent = daTbl.Rows[i]["PostContent"].ToString(),
                            PostFile = daTbl.Rows[i]["PostFile"].ToString(),
                            Date = daTbl.Rows[i]["Date"].ToString()
                        });
                    }
                }
            }
            monitorModel.PostModels = postModels;

            return View(monitorModel);
        }
    }
}