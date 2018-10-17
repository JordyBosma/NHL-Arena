using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using objects;
using App_Data;

namespace App_Data
{
    public class SignUpManager
    {
        public static Person SignUp(string username, string password)
        {
            string sql = "INSERT INTO Users (username, password) VALUES (@username, @password)";

            SQLiteConnection database;
            database = new SQLiteConnection("Data Source=App_Data\\Database.db;Version=3;");
            database.Open();

            SQLiteCommand command = new SQLiteCommand(sql, database);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.Parameters.Add(new SQLiteParameter("@password", password));

            try
            {
                command.ExecuteNonQuery();
                return LoginManager.Login(username, password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
