using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login_Session.Models;
using Login_Session.Pages.DatabaseConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace Login_Session.Pages.Users
{
    public class ViewModel : PageModel
    {
        [BindProperty]
        public List<User> User { get; set; }


        public void OnGet()
        {

            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            DatabaseConnect DBCon = new DatabaseConnect(); // your own class and method in DatabaseConnection folder
            string dbStringConnection = DBCon.DatabaseString();

            connectionStringBuilder.DataSource = dbStringConnection;
            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);

            connection.Open();


            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM UserTable";

            var reader = command.ExecuteReader();


            User = new List<User>();

            while (reader.Read())
            {
                User userRec = new User();
                userRec.Id = reader.GetInt32(0);
                userRec.FirstName = reader.GetString(1);
                userRec.UserName = reader.GetString(2);
                userRec.Role = reader.GetString(4);
                User.Add(userRec);
                
            }

        }
    }
}
