﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using objects;

namespace App_Data
{
    public class Login_Manager
    {
        /// <summary>
        /// checked de crediantials file op de username en password. geeft bij succes de permissions door aan de permissionsmanager
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Person Log_in(string username, string password)
        {
            string sql = "SELECT * " +
                "FROM Login " +
                "WHERE username = @username AND password = @password";

            List<string> resultslist = new List<string>();

            SQLiteConnection database;
            database = new SQLiteConnection("Data Source=Database.db;Version=3;");
            database.Open();


            SQLiteCommand command = new SQLiteCommand(sql, database);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.Parameters.Add(new SQLiteParameter("@password", password));

            SQLiteDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultslist.Add(Convert.ToString(reader[i]));
                    }

                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
            finally
            {
                reader.Close();
                database.Close();
            }

            try
            {
                if (username == resultslist[1] || password == resultslist[2])
                {                    
                    return new Person(Convert.ToInt32(resultslist[0]), resultslist[1]);
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }


            return null;
        }
    }
}
