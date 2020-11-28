using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login_Session.Models;
using Login_Session.Pages.DatabaseConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace Login_Session.Pages.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }

        public string Message { get; set; }

        public string SessionID;




        public IActionResult OnGet()
        {
            DatabaseConnect dbConn = new DatabaseConnect();
            string DbString = dbConn.DatabaseString();

            var connStringBuilder = new SqliteConnectionStringBuilder();
            connStringBuilder.DataSource = DbString;

            var conn = new SqliteConnection(connStringBuilder.ConnectionString);
            conn.Open();

            var command = conn.CreateCommand();
            command.CommandText = "";


            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            DatabaseConnect dbConn = new DatabaseConnect();
            string DbString = dbConn.DatabaseString();

            var connStringBuilder = new SqliteConnectionStringBuilder();
            connStringBuilder.DataSource = DbString;

            var conn = new SqliteConnection(connStringBuilder.ConnectionString);
            conn.Open();

            var command = conn.CreateCommand();

            Console.WriteLine(User.UserName);
            Console.WriteLine(User.Password);

            command.CommandText = @"SELECT FirstName, UserName, UserRole FROM UserTable WHERE UserName = @UName AND UserPassword = @Pwd";

            command.Parameters.AddWithValue("@UName", User.UserName);
            command.Parameters.AddWithValue("@Pwd", User.Password);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                User.FirstName = reader.GetString(0);
                User.UserName = reader.GetString(1);
                User.Role = reader.GetString(2);
            }

            if (!string.IsNullOrEmpty(User.FirstName))
            {
                SessionID = HttpContext.Session.Id;
                HttpContext.Session.SetString("sessionID", SessionID);
                HttpContext.Session.SetString("username", User.UserName);
                HttpContext.Session.SetString("fname", User.FirstName);

                if (User.Role == "User")
                {
                    return RedirectToPage("/UserPages/UserIndex");
                }
                else
                {
                    return RedirectToPage("/AdminPages/AdminIndex");
                }


            }
            else
            {
                Message = "Invalid Username and Password!";
                return Page();
            }

        }

    }
}
