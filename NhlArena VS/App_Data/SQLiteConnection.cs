using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace App_Data
{


/// <summary>
/// VOOR TEMPLATES ONLY!!!!!!
/// </summary>
    public static class SQLConnection
    {
                
        public static int SqlSingleCellRequest(string sql)
        {

            double aantalonafgerond = 0;
            int aantal = 0;

            SQLiteConnection database;
            database = new SQLiteConnection("Data Source=Database.db;Version=3;");
            database.Open();


            SQLiteCommand command = new SQLiteCommand(sql, database);

            SQLiteDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    aantalonafgerond = Convert.ToDouble(reader[0]);
                }
            }
            finally
            {
                reader.Close();
                database.Close();
            }

            aantal = Convert.ToInt32(Math.Round(aantalonafgerond, MidpointRounding.AwayFromZero));

            return aantal;
        }

        public static string[] SqlSingleRowRequest(string sql)
        {
            List<string> resultslist = new List<string>();

            SQLiteConnection database;
            database = new SQLiteConnection("Data Source=Database.db;Version=3;");
            database.Open();


            SQLiteCommand command = new SQLiteCommand(sql, database);

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
            finally
            {
                reader.Close();
                database.Close();
            }

            string[] results = resultslist.ToArray();
            return results;
        }

        public static string[] SqlSingleColumRequest(string sql)
        {

            List<string> resultslist = new List<string>();

            SQLiteConnection database;
            database = new SQLiteConnection("Data Source=Database.db;Version=3;");
            database.Open();


            SQLiteCommand command = new SQLiteCommand(sql, database);

            SQLiteDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    resultslist.Add(Convert.ToString(reader[0]));

                }
            }
            finally
            {
                reader.Close();
                database.Close();
            }

            string[] results = resultslist.ToArray();
            return results;
        }

        public static void SqlInsert(string sql)
        {

            SQLiteConnection database;
            database = new SQLiteConnection("Data Source=Database.db;Version=3;");
            database.Open();


            SQLiteCommand command = new SQLiteCommand(sql, database);
            command.ExecuteNonQuery();

        }


    }
}