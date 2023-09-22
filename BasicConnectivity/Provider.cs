using System.Data.SqlClient;

namespace BasicConnectivity;

public class Provider
{
    // String koneksi ke database SQL Server.
    private static readonly string connectionString ="Data Source=DESKTOP-0KRUQ0V;Integrated Security=True;Database=db_hr_dts;Connect Timeout=30;";

    // Mengembalikan objek SqlConnection untuk terhubung ke database.
    public static SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
    
    // Mengembalikan objek SqlCommand untuk menjalankan perintah SQL pada database yang terhubung.
    public static SqlCommand GetCommand()
    {
        return new SqlCommand();
    }
    
    

}