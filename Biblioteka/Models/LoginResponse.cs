using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteka.Models
{
    public class LoginResponse
    {



        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }


        [Required]
        [Display(Name = "Hasło")]
        public string Password { get; set; }



        public bool IsValidAdmin(string login, string password)
        {
            var conn =
                new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Użytkownicy.accdb");
            var cmd = new OleDbCommand("SELECT [Login] FROM [Users] " + @"WHERE [Login] = @l AND [Hasło] = @p AND [Prawa] = @a") {Connection = conn};
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new OleDbParameter("@l", OleDbType.VarWChar)).Value = login;
            cmd.Parameters.Add(new OleDbParameter("@p", OleDbType.VarWChar)).Value = password;
            cmd.Parameters.Add(new OleDbParameter("@a", OleDbType.VarWChar)).Value = "Administrator";
            conn.Open();

            var reader = cmd.ExecuteReader();

            if (reader != null && reader.HasRows)
            {
                reader.Dispose();
                cmd.Dispose();
                return true;
            }
            else
            {
                reader?.Dispose();
                cmd.Dispose();
                return false;
            }

        }

        public bool IsValidModerator(string login, string password)
        {
            var conn =
                new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|Użytkownicy.accdb");
            var cmd = new OleDbCommand("SELECT [Login] FROM [Users] " + @"WHERE [Login] = @l AND [Hasło] = @p AND [Prawa] = @a") { Connection = conn };
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new OleDbParameter("@l", OleDbType.VarWChar)).Value = login;
            cmd.Parameters.Add(new OleDbParameter("@p", OleDbType.VarWChar)).Value = password;
            cmd.Parameters.Add(new OleDbParameter("@a", OleDbType.VarWChar)).Value = "Moderator";
            conn.Open();

            var reader = cmd.ExecuteReader();

            if (reader != null && reader.HasRows)
            {
                reader.Dispose();
                cmd.Dispose();
                return true;
            }
            else
            {
                reader?.Dispose();
                cmd.Dispose();
                return false;
            }

        }

    }
}