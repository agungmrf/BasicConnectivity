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

        // Mengambil semua data dari tabel "regions".
        //GetAllRegions();

        // Menambahkan data baru ke dalam tabel "regions.
        //InsertRegion(8, "Banda Neira");

        // Mengambil data dari tabel "regions" berdasarkan ID.
        /*int regionId = 8;
        GetRegionsById(regionId);*/

        // Menghapus data dari tabel "regions" berdasarkan ID.
        /*int regionIdDelete = 9;
        DeleteRegion(regionIdDelete);*/

        // Memperbarui data dalam tabel "regions".
        int regionIdUpdate = 7;
        string newRegionName = "Kuala Namu";
        UpdateRegion(regionIdUpdate, newRegionName);
    }


    // Mengambil semua data dari tabel "regions".
    public static void GetAllRegions()
    {
        // Menginisialisasi object koneksi SqlConnection
        using var connection = new SqlConnection(connectionString);

        // Menginisialisasi object perintah SQL
        using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "SELECT * FROM regions";

        try
        {
            connection.Open(); // Membuka koneksi.

            using var reader = command.ExecuteReader();

            // Memeriksa apakah ada baris hasil dari query.
            if (reader.HasRows)
                while (reader.Read())
                {
                    Console.WriteLine("Id: " + reader.GetInt32(0));
                    Console.WriteLine("Name: " + reader.GetString(1));
                }
            else
                Console.WriteLine("No rows found.");

            reader.Close();
            connection.Close(); // Menutup koneksi.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Mengambil data dari tabel "regions" berdasarkan ID.
    public static void GetRegionsById(int id)
    {
        // Menginisialisasi object koneksi SqlConnection
        using var connection = new SqlConnection(connectionString);

        // Menginisialisasi object perintah SQL
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

            connection.Open(); // Membuka koneksi.
            using var reader = command.ExecuteReader();

            // Memeriksa apakah ada baris hasil dari query.
            if (reader.HasRows)
                while (reader.Read())
                {
                    Console.WriteLine("Id: " + reader.GetInt32(0));
                    Console.WriteLine("Name: " + reader.GetString(1));
                }
            else
                Console.WriteLine("No rows found.");

            reader.Close();
            connection.Close(); // Menutup koneksi.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }


    // Menambahkan data ke dalam tabel "regions".
    public static void InsertRegion(int id, string name)
    {
        // Menginisialisasi object koneksi SqlConnection
        using var connection = new SqlConnection(connectionString);

        // Menginisialisasi object perintah SQL
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

            connection.Open(); // Membuka koneksi.
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction; // Menetapkan object transaksi dengan object perintah SQL.

                var result = command.ExecuteNonQuery(); // Menjalankan perintah SQL dan menghitung berapa banyak baris data yang dipengaruhi oleh perintah tersebut.

                transaction.Commit();
                connection.Close(); // Menutup koneksi.

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
                transaction.Rollback(); // Membatalkan transaksi yang sedang berlangsung dalam basis data.
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Memperbarui data di tabel "regions" berdasarkan ID.
    public static void UpdateRegion(int id,  string name)
    {
        // Menginisialisasi object koneksi SqlConnection
        using var connection = new SqlConnection(connectionString);

        // Menginisialisasi object perintah SQL
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

            connection.Open(); // Membuka koneksi.
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction; // Menetapkan object transaksi dengan object perintah SQL.

                var result = command.ExecuteNonQuery(); // Menjalankan perintah SQL dan menghitung berapa banyak baris data yang dipengaruhi oleh perintah tersebut.

                transaction.Commit();
                connection.Close(); // Menutup koneksi.

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
                transaction.Rollback(); // Membatalkan transaksi yang sedang berlangsung dalam basis data.
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Menghapus data dari tabel "regions" berdasarkan ID.
    public static void DeleteRegion(int id)
    {
        // Menginisialisasi object koneksi SqlConnection
        using var connection = new SqlConnection(connectionString);

        // Menginisialisasi object perintah SQL
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

            connection.Open(); // Membuka koneksi.
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction; // Menetapkan object transaksi dengan object perintah SQL.

                var result = command.ExecuteNonQuery(); // Menjalankan perintah SQL dan menghitung berapa banyak baris data yang dipengaruhi oleh perintah tersebut.

                transaction.Commit();
                connection.Close(); // Menutup koneksi.

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
                transaction.Rollback(); // Membatalkan transaksi yang sedang berlangsung dalam basis data.
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
