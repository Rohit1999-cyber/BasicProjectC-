using MySql.Data.MySqlClient;
using System;

public class MySQLCRUD
{
    private readonly string ConnectionString;

    public MySQLCRUD(string connectionString)
    {
        ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public void dbconnect()
    {
        using (MySqlConnection con = new MySqlConnection(ConnectionString))
        {
            try
            {
                con.Open();
                Console.WriteLine("MySQL is Connected Successfully");
                string CreateTable = @"CREATE TABLE IF NOT EXISTS STUDENT (
                                        NAME VARCHAR(20) NOT NULL,
                                        BRANCH VARCHAR(50),
                                        ROLL INT NOT NULL,
                                        SECTION VARCHAR(5),
                                        AGE INT
                                      )";
                using (MySqlCommand cmd = new MySqlCommand(CreateTable, con))
                {
                    cmd.ExecuteNonQuery();
                }

                CleanupStudentDuplicates(con);

                Console.WriteLine("Table STUDENT created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    public void dbinsert(string name, string branch, int roll, string section, int age)
    {
        using (MySqlConnection con = new MySqlConnection(ConnectionString))
        {
            try
            {
                con.Open();
                string query = "INSERT INTO STUDENT(NAME, BRANCH, ROLL, SECTION, AGE) VALUES(@name,@branch,@roll,@section,@age) " +
                               "ON DUPLICATE KEY UPDATE NAME=VALUES(NAME), BRANCH=VALUES(BRANCH), SECTION=VALUES(SECTION), AGE=VALUES(AGE)";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@branch", branch);
                    cmd.Parameters.AddWithValue("@roll", roll);
                    cmd.Parameters.AddWithValue("@section", section);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Record inserted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    private void CleanupStudentDuplicates(MySqlConnection con)
    {
        try
        {
            using (var cmd = new MySqlCommand("DROP TEMPORARY TABLE IF EXISTS tmp_students", con))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new MySqlCommand(@"CREATE TEMPORARY TABLE tmp_students AS
                                                SELECT MIN(NAME) AS NAME,
                                                       MIN(BRANCH) AS BRANCH,
                                                       ROLL,
                                                       MIN(SECTION) AS SECTION,
                                                       MIN(AGE) AS AGE
                                                FROM STUDENT
                                                GROUP BY ROLL", con))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new MySqlCommand("TRUNCATE TABLE STUDENT", con))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new MySqlCommand("INSERT INTO STUDENT(NAME, BRANCH, ROLL, SECTION, AGE) SELECT NAME, BRANCH, ROLL, SECTION, AGE FROM tmp_students", con))
            {
                cmd.ExecuteNonQuery();
            }

            try
            {
                using (var cmd = new MySqlCommand("ALTER TABLE STUDENT ADD UNIQUE INDEX uniq_roll (ROLL)", con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex) when (ex.Number == 1061)
            {
                // Unique index already exists, ignore.
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Warning: failed to cleanup duplicate STUDENT rows: " + ex.Message);
        }
    }

    public void GetStudents()
    {   
        using (MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM STUDENT";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
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

    public void UpdateStudentAge(int roll, int newAge)
    {
        using (MySqlConnection conn = new MySqlConnection(ConnectionString))
        {   
            try
            {
                conn.Open();
                string query = "UPDATE STUDENT SET AGE=@age WHERE ROLL=@roll";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@age", newAge);
                    cmd.Parameters.AddWithValue("@roll", roll);
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Record Updated Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    public void DeleteStudent(int roll)
    {
        using (MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            try
            {
                conn.Open();
                string query = "DELETE FROM STUDENT WHERE ROLL=@roll";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roll", roll);
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Record Deleted Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
