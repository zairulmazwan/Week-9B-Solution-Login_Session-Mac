using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Login_Session.Models;
using Login_Session.Pages.DatabaseConnection;
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
        public void OnGet()
        {
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
            

            return RedirectToPage("/Index");
        }
    }
}
