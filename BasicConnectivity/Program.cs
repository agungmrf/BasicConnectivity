using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BasicConnectivity;

public class Program
{
    // String koneksi ke database SQL Server.
    static string connectionString = "Data Source=DESKTOP-0KRUQ0V;Database=DB_Employee;Connect Timeout=30;Integrated Security=True";

    // Objek SqlConnection
    static SqlConnection connection;

    private static void Main()
    {
        /*
        // Membuat objek SqlConnection dengan menggunakan string connectionString.
        connection = new SqlConnection(connectionString);
        try
        {
            // Membuka koneksi ke database.
            connection.Open();
            Console.WriteLine("Connection opened successfully");

            // Menutup koneksi setelah digunakan.
            connection.Close();
        } catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        */

        //GetAllRegions();

        //InsertRegion(8, "Banda Neira");

        /*int regionId = 8;
        GetRegionsById(regionId);*/

        /*int regionIdDelete = 9;
        DeleteRegion(regionIdDelete);*/

        int regionIdUpdate = 7;
        string newRegionName = "Kuala Namu";
        UpdateRegion(regionIdUpdate, newRegionName);
    }


    // GET ALL: Region
    public static void GetAllRegions()
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "SELECT * FROM regions";

        try
        {
            connection.Open();

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                {
                    Console.WriteLine("Id: " + reader.GetInt32(0));
                    Console.WriteLine("Name: " + reader.GetString(1));
                }
            else
                Console.WriteLine("No rows found.");

            reader.Close();
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // GET BY ID: Region
    public static void GetRegionsById(int id)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "SELECT * FROM regions WHERE Id = @id";

        try
        {
            var pId = new SqlParameter();
            pId.ParameterName = "id";
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;
            command.Parameters.Add(pId);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                {
                    Console.WriteLine("Id: " + reader.GetInt32(0));
                    Console.WriteLine("Name: " + reader.GetString(1));
                }
            else
                Console.WriteLine("No rows found.");

            reader.Close();
            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }


    // INSERT: Region
    public static void InsertRegion(int id, string name)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "INSERT INTO regions VALUES (@id, @region_name)";

        try
        {
            var pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;
            command.Parameters.Add(pId);

            var pName = new SqlParameter();
            pName.ParameterName = "@region_name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;
            command.Parameters.Add(pName);

            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;

                var result = command.ExecuteNonQuery();

                transaction.Commit();
                connection.Close();

                switch (result)
                {
                    case >= 1:
                        Console.WriteLine($"Insert Success ID: {id} Region Name: {name}");
                        break;
                    default:
                        Console.WriteLine("Insert Failed");
                        break;
                }
            }
            catch (Exception ex) 
            {
                transaction.Rollback();
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // UPDATE: Region
    public static void UpdateRegion(int id,  string name)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "UPDATE regions SET region_name = @region_name WHERE Id = @id";

        try
        {
            var pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;
            command.Parameters.Add(pId);

            var pName = new SqlParameter();
            pName.ParameterName = "@region_name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;
            command.Parameters.Add(pName);

            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;

                var result = command.ExecuteNonQuery();

                transaction.Commit();
                connection.Close();

                switch (result)
                {
                    case >= 1:
                        Console.WriteLine($"Update Success ID: {id} New Region Name: {name}");
                        break;
                    default:
                        Console.WriteLine($"No record found with ID: { id}");
                        break;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // DELETE: Region
    public static void DeleteRegion(int id)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "DELETE FROM regions WHERE Id = @id";

        try
        {
            var pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;
            command.Parameters.Add(pId);

            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;

                var result = command.ExecuteNonQuery();

                transaction.Commit();
                connection.Close();

                switch (result)
                {
                    case >= 1:
                        Console.WriteLine($"Delete Success ID: {id}");
                        break;
                    default:
                        Console.WriteLine($"No record found with ID: {id}");
                        break;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
