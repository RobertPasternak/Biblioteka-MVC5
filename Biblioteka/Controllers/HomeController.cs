using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Biblioteka.Models;

namespace Biblioteka.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult LoginForm()
        {
            return View();
        }


        [HttpPost]
        public ActionResult LoginForm(LoginResponse loginResponse)
        {
            if (ModelState.IsValid)
            {
                if (loginResponse.IsValidAdmin(loginResponse.Login, loginResponse.Password))
                {
                    //FormsAuthentication.SetAuthCookie(loginResponse.Login, true);

                    var conn =
                        new OleDbConnection(
                            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Użytkownicy.accdb");
                    var cmd = new OleDbCommand("SELECT * FROM [Users]") {Connection = conn};
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    var usersModel = new List<UsersData>();
                    while (reader != null && reader.Read())
                    {
                        var usersData = new UsersData
                        {
                            Id = (int) reader["Identyfikator"],
                            Login = (string) reader["Login"],
                            Password = (string) reader["Hasło"],
                            Privileges = (string) reader["Prawa"]
                        };

                        usersModel.Add(usersData);
                    }
                    conn.Close();
                    return View("AdminPanelForm", usersModel);
                }
                else if (loginResponse.IsValidModerator(loginResponse.Login, loginResponse.Password))
                {
                    //FormsAuthentication.SetAuthCookie(loginResponse.Login, true);

                    var conn =
                       new OleDbConnection(
                           "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Awarie.accdb");
                    var cmd = new OleDbCommand("SELECT * FROM [Zgloszenie]") { Connection = conn };
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    var failuresModel = new List<FailuresData>();
                    while (reader != null && reader.Read())
                    {
                        var failuresData = new FailuresData
                        {
                            Id = (int)reader["Identyfikator"],
                            EntryDate = (string)reader["Data"],
                            Topic = (string)reader["Temat"],
                            Description = (string)reader["Opis"],
                            Floor = (int)reader["Pietro"],
                            Area = (string)reader["Strefa"],
                            Worksite = (string)reader["Stanowisko"],
                            Status = (bool)reader["Status"]
                        };

                        failuresModel.Add(failuresData);
                    }
                    conn.Close();
                    return View("ModeratorPanelForm", failuresModel);
                }
                else
                {
                    ModelState.AddModelError("", "Błędne dane logowania.");
                }
            }
            return View();
        }


        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult DeleteUserData(int id)
        {
            var conn =
                new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Użytkownicy.accdb");

            var delCmd = new OleDbCommand("DELETE FROM [Users] WHERE [Identyfikator] = @i") {Connection = conn};
            delCmd.Parameters.Clear();
            delCmd.Parameters.Add(new OleDbParameter("@i", OleDbType.VarWChar)).Value = id;

            conn.Open();

            delCmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("AdminPanelForm");
        }


        public ActionResult DeleteFailureData(int id)
        {
            var conn =
                new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Awarie.accdb");

            var delCmd = new OleDbCommand("DELETE FROM [Zgloszenie] WHERE [Identyfikator] = @i") { Connection = conn };
            delCmd.Parameters.Clear();
            delCmd.Parameters.Add(new OleDbParameter("@i", OleDbType.VarWChar)).Value = id;
            conn.Open();
            delCmd.ExecuteNonQuery();
            conn.Close();



            delCmd = new OleDbCommand("DELETE FROM [Komentarze] WHERE [IdAwarii] = @i") { Connection = conn };
            delCmd.Parameters.Clear();
            delCmd.Parameters.Add(new OleDbParameter("@i", OleDbType.VarWChar)).Value = id;
            conn.Open();
            delCmd.ExecuteNonQuery();
            conn.Close();


            return RedirectToAction("ModeratorPanelForm");
        }


        public ActionResult ChangeStatusFailureData(int id, bool status)
        {

            var conn =
                new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Awarie.accdb");

            var updCmd = new OleDbCommand("UPDATE [Zgloszenie] SET [Status] = @s WHERE [Identyfikator] = @i") { Connection = conn };
           updCmd.Parameters.Clear();
            updCmd.Parameters.Add(new OleDbParameter("@s", OleDbType.Boolean)).Value = !status;
            updCmd.Parameters.Add(new OleDbParameter("@i", OleDbType.VarWChar)).Value = id;


            conn.Open();

            updCmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("ModeratorPanelForm");
        }


        public ActionResult AdminPanelForm()
        {
            var conn =
                new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Użytkownicy.accdb");
            var cmd = new OleDbCommand("SELECT * FROM [Users]") {Connection = conn};
            conn.Open();
            var reader = cmd.ExecuteReader();

            var usersModel = new List<UsersData>();
            while (reader != null && reader.Read())
            {
                var usersData = new UsersData
                {
                    Id = (int) reader["Identyfikator"],
                    Login = (string) reader["Login"],
                    Password = (string) reader["Hasło"],
                    Privileges = (string) reader["Prawa"]
                };

                usersModel.Add(usersData);
            }
            ModelState.AddModelError("", "Akcja wykonana.");
            conn.Close();
            return View("AdminPanelForm", usersModel);
        }



        public ActionResult ModeratorPanelForm()
        {

            var conn =
                      new OleDbConnection(
                          "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Awarie.accdb");
            var cmd = new OleDbCommand("SELECT * FROM [Zgloszenie]") { Connection = conn };
            conn.Open();
            var reader = cmd.ExecuteReader();

            var failuresModel = new List<FailuresData>();
            while (reader != null && reader.Read())
            {
                var failuresData = new FailuresData
                {
                    Id = (int)reader["Identyfikator"],
                    EntryDate = (string)reader["Data"],
                    Topic = (string)reader["Temat"],
                    Description = (string)reader["Opis"],
                    Floor = (int)reader["Pietro"],
                    Area = (string)reader["Strefa"],
                    Worksite = (string)reader["Stanowisko"],
                    Status = (bool)reader["Status"]
                };
                failuresModel.Add(failuresData);
            }
            ModelState.AddModelError("", "Akcja wykonana.");
            conn.Close();
            return View("ModeratorPanelForm", failuresModel);
        }


        
        public ActionResult BrowseCommentsForm(int id)
        {
            var conn =
                     new OleDbConnection(
                         "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Awarie.accdb");
            var cmd = new OleDbCommand("SELECT * FROM [Zgloszenie] WHERE [Identyfikator] = @i ") { Connection = conn };
            cmd.Parameters.Add(new OleDbParameter("@i", OleDbType.VarWChar)).Value = id;
            conn.Open();
            var reader = cmd.ExecuteReader();

            var browseCommentsModel = new List<FailuresData>();
            while (reader != null && reader.Read())
            {
                var browseCommentsData = new FailuresData
                {
                    Id = (int)reader["Identyfikator"],
                    EntryDate = (string)reader["Data"],
                    Topic = (string)reader["Temat"],
                    Description = (string)reader["Opis"],
                    Floor = (int)reader["Pietro"],
                    Area = (string)reader["Strefa"],
                    Worksite = (string)reader["Stanowisko"],
                    Status = (bool)reader["Status"]
                };
                browseCommentsModel.Add(browseCommentsData);
            }
            conn.Close();


            var newcmd = new OleDbCommand("SELECT * FROM [Komentarze] WHERE [IdAwarii] = @i ") { Connection = conn };
            newcmd.Parameters.Add(new OleDbParameter("@i", OleDbType.VarWChar)).Value = id;

            conn.Open();
            reader = newcmd.ExecuteReader();

            while (reader != null && reader.Read())
            {
                var browseCommentsData = new FailuresData
                {
                    Id = -1,
                    Comment = (string)reader["Komentarz"]
                };
                browseCommentsModel.Add(browseCommentsData);
            }


            conn.Close();                

            return View("BrowseCommentsForm", browseCommentsModel);
        }



        [HttpGet]
        public ActionResult AddCommentForm()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddCommentForm(AddComment addComment)
        {
            if (ModelState.IsValid)
            {

                var conn =
    new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Awarie.accdb");

                var addCmd = new OleDbCommand("INSERT INTO [Komentarze] ([Komentarz], [IdAwarii]) VALUES (@k, @i)") { Connection = conn };
                addCmd.Parameters.Clear();
                addCmd.Parameters.Add(new OleDbParameter("@k", OleDbType.VarWChar)).Value = addComment.User + ": "  + addComment.NewComment;
                addCmd.Parameters.Add(new OleDbParameter("@i", OleDbType.VarWChar)).Value = addComment.Id;


                conn.Open();

                addCmd.ExecuteNonQuery();
                conn.Close();

                return RedirectToAction("ModeratorPanelForm");
            }
            else
            {
                
                return View();
            }
        }


        [HttpGet]
        public ActionResult AddUserForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUserForm(AddUser addUser)
        {
            if (ModelState.IsValid)
            {
                
                var conn =
                    new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Użytkownicy.accdb");

                var addCmd =
                    new OleDbCommand("INSERT INTO [Users] ([Login], [Hasło], [Prawa]) VALUES (@l, @p, @a)")
                    {
                        Connection = conn
                    };
                addCmd.Parameters.Clear();

                addCmd.Parameters.Add(new OleDbParameter("@l", OleDbType.VarWChar)).Value = addUser.Login;
                addCmd.Parameters.Add(new OleDbParameter("@p", OleDbType.VarWChar)).Value = addUser.Password;
                addCmd.Parameters.Add(new OleDbParameter("@a", OleDbType.VarWChar)).Value = addUser.Privileges;
                conn.Open();

                addCmd.ExecuteNonQuery();
                conn.Close();
                return RedirectToAction("AdminPanelForm");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult AddFailureForm()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddFailureForm(AddFailure addFailure)
        {
            if (ModelState.IsValid)
            {
                var conn =
                    new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Awarie.accdb");
              
                var addCmd =
                    new OleDbCommand("INSERT INTO [Zgloszenie] ([Data], [Temat], [Opis], [Pietro], [Strefa], [Stanowisko], [Status]) VALUES (@data, @opis, @komentarz, @pietro, @strefa, @stanowisko, @status)")
                    {
                        Connection = conn
                    };
                addCmd.Parameters.Clear();

                addCmd.Parameters.Add(new OleDbParameter("@data", OleDbType.VarWChar)).Value = addFailure.EntryDate;
              addCmd.Parameters.Add(new OleDbParameter("@opis", OleDbType.VarWChar)).Value = addFailure.Topic;
                addCmd.Parameters.Add(new OleDbParameter("@komentarz", OleDbType.VarWChar)).Value = addFailure.Description;
                 addCmd.Parameters.Add(new OleDbParameter("@pietro", OleDbType.Integer)).Value = addFailure.Floor;
                addCmd.Parameters.Add(new OleDbParameter("@strefa", OleDbType.VarWChar)).Value = addFailure.Area;
                 addCmd.Parameters.Add(new OleDbParameter("@stanowisko", OleDbType.VarWChar)).Value = addFailure.Worksite;
                addCmd.Parameters.Add(new OleDbParameter("@status", OleDbType.Boolean)).Value = addFailure.Status;
                conn.Open();

                addCmd.ExecuteNonQuery();
                conn.Close();
                return View("Thanks", addFailure);
            }
            else
            {
                return View();
            }
        }


       
        
    }
}
 