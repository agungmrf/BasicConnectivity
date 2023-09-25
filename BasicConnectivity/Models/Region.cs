using System.Data.SqlClient;

namespace BasicConnectivity.Models;

public class Region
{
    // Properti yang merepresentasikan kolom-kolom dalam tabel 'Region'
    public int Id { get; set; }
    public string Name { get; set; }
    
    public override string ToString()
    {
        return $"{Id} - {Name}";
    }
    
    // Mengambil semua data dari tabel "regions".
    public List<Region> GetAll()
    {
        var regions = new List<Region>();
        
        using var connection = Provider.GetConnection();
        using var command = Provider.GetCommand();

        command.Connection = connection;
        command.CommandText = "SELECT * FROM regions";
        
        try
        {
            connection.Open(); // Membuka koneksi.
            using var reader = command.ExecuteReader();

            // Memeriksa apakah ada baris hasil dari query.
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // Membaca data dari hasil query dan menambahkan ke daftar regions.
                    regions.Add(new Region
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }

                reader.Close();
                connection.Close();

                return regions;
            }
            reader.Close();
            connection.Close(); // Menutup koneksi.

            return new List<Region>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<Region>(); // Mengembalikan daftar kosong jika terjadi kesalahan.
        }
    }

    // Mengambil data dari tabel "regions" berdasarkan ID.
    public Region GetById(int id)
    {
        using var connection = Provider.GetConnection();
        using var command = Provider.GetCommand();

        command.Connection = connection;
        command.CommandText = "SELECT * FROM regions WHERE Id = @id";
        
        try
        {
            command.Parameters.Add(new SqlParameter("@id", id));
            
            connection.Open(); // Membuka koneksi.
            using var reader = command.ExecuteReader();
            
            // Memeriksa apakah ada baris hasil dari query.
            if (reader.HasRows)
            {
                Region region = null;
                
                while (reader.Read())
                {
                    // Membaca data dari hasil query dan menginisialisasi objek Region.
                    region = new Region
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                }
                reader.Close();
                connection.Close(); // Menutup koneksi.

                return region;
            }
            reader.Close();
            connection.Close(); // Menutup koneksi.

            return null; // Mengembalikan null jika tidak ada data yang ditemukan.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null; // Mengembalikan null jika ada kesalahan.
        }
    }
    
    // Menambahkan data ke dalam tabel "regions".
    public string Insert(Region region)
    {
        using var connection = Provider.GetConnection();
        using var command = Provider.GetCommand();

        command.Connection = connection;
        command.CommandText = "INSERT INTO regions VALUES (@name)";

        try
        {
            command.Parameters.Add(new SqlParameter("@name", region.Name));

            connection.Open(); // Membuka koneksi.
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction; // Menetapkan object transaksi dengan object perintah SQL.

                var result = command.ExecuteNonQuery(); // Menjalankan perintah SQL dan menghitung berapa banyak baris data yang dipengaruhi oleh perintah tersebut.

                transaction.Commit();
                connection.Close(); // Menutup koneksi.
                
                return result.ToString();
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Membatalkan transaksi yang sedang berlangsung dalam basis data.
                return $"Error Transaction: {ex.Message}";
            }
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    // Memperbarui data di tabel "regions" berdasarkan ID.
    public string Update(int id, string name)
    {
        using var connection = Provider.GetConnection();
        using var command = Provider.GetCommand();

        command.Connection = connection;
        command.CommandText = "UPDATE regions SET name = @name WHERE Id = @id";

        try
        {
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@name", name));

            connection.Open(); // Membuka koneksi.

            using var transaction = connection.BeginTransaction();
            
            try
            {
                command.Transaction = transaction; // Menetapkan object transaksi dengan object perintah SQL.
                
                var result = command.ExecuteNonQuery(); // Menjalankan perintah SQL dan menghitung berapa banyak baris data yang dipengaruhi oleh perintah tersebut.

                transaction.Commit();
                connection.Close();
                
                return result.ToString();
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Membatalkan transaksi yang sedang berlangsung dalam basis data.
                return $"Error Transaction: {ex.Message}";
            }
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    // Menghapus data dari tabel "regions" berdasarkan ID.
    public string Delete(int id)
    {
        using var connection = Provider.GetConnection();
        using var command = Provider.GetCommand();

        command.Connection = connection;
        command.CommandText = "DELETE FROM regions WHERE Id = @id";

        try
        {
            command.Parameters.Add(new SqlParameter("@id", id));

            connection.Open(); // Membuka koneksi.
            
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction; // Menetapkan object transaksi dengan object perintah SQL.

                var result = command.ExecuteNonQuery(); // Menjalankan perintah SQL dan menghitung berapa banyak baris data yang dipengaruhi oleh perintah tersebut.
                
                transaction.Commit(); // Commit transaksi jika berhasil.
                connection.Close(); // Menutup koneksi.

                return result.ToString();
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Membatalkan transaksi yang sedang berlangsung dalam basis data.
                return $"Error: {ex.Message}";
            }
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}