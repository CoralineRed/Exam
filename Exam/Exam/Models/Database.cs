using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class Database
    {
        static string connString { get; set; }
            = "Host=localhost;Username=postgres;Password=postgresql;Database=exam";

        public static void Save(Dictionary<string, string> data, string domain)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    foreach (var link in data)
                    {
                        try
                        {
                            cmd.CommandText = $"INSERT INTO links VALUES ('{link.Key}', '{link.Value}', '{domain}');";
                            cmd.ExecuteNonQuery();
                        }
                        catch { }
                    }
                }
            }
        }

        public static bool Exists(string link)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT link FROM links", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        if (link == (string)reader.GetValue(0))
                            return true;
            }
            return false;
        }

        public static Dictionary<string, string> SelectByDomain(string domain)
        {
            var results = new Dictionary<string, string>();
            var res = new object[3];
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"SELECT * FROM links WHERE domain = '{domain}'", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        reader.GetValues(res);
                        results.Add((string)res[0], (string)res[1]);
                    }
                return results;
            }
        }
    }
}
