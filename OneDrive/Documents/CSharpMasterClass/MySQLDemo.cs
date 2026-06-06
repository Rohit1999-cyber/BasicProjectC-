using MySql.Data.MySqlClient;
using System;
class MySQLDemo
{
    private readonly string ConnectionString;

    public MySQLDemo(string connectionString)
    {
        ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public void connect()
    {
        using (MySqlConnection con = new MySqlConnection(ConnectionString))
        {
            try
            {
                con.Open();
                Console.WriteLine("MySQL is Connected Successfully");

                // Ensure users table exists
                string createUsers = @"CREATE TABLE IF NOT EXISTS users (
                                        id INT AUTO_INCREMENT PRIMARY KEY,
                                        username VARCHAR(100) NOT NULL
                                      )";
                using (var cmd = new MySqlCommand(createUsers, con))
                {
                    cmd.ExecuteNonQuery();
                }

                // Insert a sample user if table is empty
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM users", con))
                {
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        using (var ins = new MySqlCommand("INSERT INTO users(username) VALUES(@u)", con))
                        {
                            ins.Parameters.AddWithValue("@u", "Alice");
                            ins.ExecuteNonQuery();
                        }
                    }
                }

                // Read and print users
                Console.WriteLine("Users:");
                using (var cmd = new MySqlCommand("SELECT username FROM users", con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["username"]);
                    }
                }

                // Read and print students
                Console.WriteLine("Students:");
                using (var cmd = new MySqlCommand("SELECT NAME, BRANCH, ROLL, SECTION, AGE FROM STUDENT", con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["NAME"]} | {reader["BRANCH"]} | {reader["ROLL"]} | {reader["SECTION"]} | {reader["AGE"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
