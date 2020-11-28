using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Login_Session.Models;
using Login_Session.Pages.DatabaseConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace Login_Session.Pages.Users
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }

        public List<string> URole { get; set; } = new List<string> { "User", "Admin" };

        public string UserName;
        public const string SessionKeyName1 = "username";


        public string FirstName;
        public const string SessionKeyName2 = "fname";

        public string SessionID;
        public const string SessionKeyName3 = "sessionID";

        public void OnGet()
        {
            //get the session first!
            UserName = HttpContext.Session.GetString(SessionKeyName1);
            FirstName = HttpContext.Session.GetString(SessionKeyName2);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);
        }

        public IActionResult OnPost()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            DatabaseConnect DBCon = new DatabaseConnect(); // your own class and method in DatabaseConnection folder
            string dbStringConnection = DBCon.DatabaseString();

            connectionStringBuilder.DataSource = dbStringConnection;
            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);

            connection.Open();

            var command = connection.CreateCommand();

            Console.WriteLine(User.FirstName);
            Console.WriteLine(User.UserName);
            Console.WriteLine(User.Password);
            Console.WriteLine(User.Role);

           
                
                command.CommandText = @"INSERT INTO UserTable (FirstName, UserName, UserPassword, UserRole) VALUES (@FName, @UName, @Pwd, @Role)";

                command.Parameters.AddWithValue("@FName",User.FirstName);
                command.Parameters.AddWithValue("@UName", User.UserName);
                command.Parameters.AddWithValue("@Pwd", User.Password);
                command.Parameters.AddWithValue("@Role", User.Role);
                command.ExecuteNonQuery();
            

            return RedirectToPage("/AdminPages/AdminIndex");
        }
    }
}
