using System.Data.SqlClient;

namespace BasicConnectivity;

public class Jobs
{
    // Properti yang merepresentasikan kolom-kolom dalam tabel 'jobs'
    public string Id { get; set; }
    public string Title { get; set; }
    public decimal Min_Salary { get; set; }
    public decimal Max_Salary { get; set; }
    
    // Method untuk menggabungkan data properti menjadi sebuah string
    public override string ToString()
    {
        return $"{Id} - {Title} - {Min_Salary} - {Max_Salary}";
    }

    public List<Jobs> GetAll()
    {
        var jobs = new List<Jobs>();
        
        using var connection = Provider.GetConnection(); // Menggunakan kelas Provider untuk mendapatkan koneksi.
        using var command = Provider.GetCommand(); // Menggunakan kelas Provider untuk mendapatkan perintah SQL.

        command.Connection = connection;
        command.CommandText = "SELECT * FROM jobs";

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    jobs.Add(new Jobs
                    {
                        Id = reader.GetString(0),
                        Title = reader.GetString(1),
                        Min_Salary = reader.GetInt32(2),
                        Max_Salary = reader.GetInt32(3),
                    });
                }

                reader.Close();
                connection.Close();

                return jobs;
            }

            reader.Close();
            connection.Close();

            return new List<Jobs>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<Jobs>();
        }
    }
    
    public Jobs GetById(string id)
    {
        using var connection = Provider.GetConnection();
        using var command = Provider.GetCommand();

        command.Connection = connection;
        command.CommandText = "SELECT * FROM jobs WHERE Id = @Id";
    
        try
        {
            command.Parameters.Add(new SqlParameter("@id", id));
        
            connection.Open(); // Membuka koneksi.
            using var reader = command.ExecuteReader();
        
            // Memeriksa apakah ada baris hasil dari query.
            if (reader.HasRows)
            {
                Jobs job = null;
            
                while (reader.Read())
                {
                    job = new Jobs
                    {
                        Id = reader.GetString(0),
                        Title = reader.GetString(1),
                        Min_Salary = reader.GetInt32(2),
                        Max_Salary = reader.GetInt32(3),
                    };
                }
                reader.Close();
                connection.Close(); // Menutup koneksi.

                return job;
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

     public string Insert(string title, int minSalary, int maxSalary)
    {
        using var connection = Provider.GetConnection();
        using var command = Provider.GetCommand();

        command.Connection = connection;
        command.CommandText = "INSERT INTO jobs (Title, Min_Salary, Max_Salary) VALUES (@title, @minSalary, @maxSalary)";

        try
        {
            command.Parameters.Add(new SqlParameter("@title", title));
            command.Parameters.Add(new SqlParameter("@minSalary", minSalary));
            command.Parameters.Add(new SqlParameter("@maxSalary", maxSalary));

            connection.Open(); // Membuka koneksi.
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction; // Menetapkan object transaksi dengan object perintah SQL.

                var result = command.ExecuteNonQuery();

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

    public string Update(string id, string title, int minSalary, int maxSalary)
    {
        using var connection = Provider.GetConnection();
        using var command = Provider.GetCommand();

        command.Connection = connection;
        command.CommandText = "UPDATE jobs SET Title = @title, Min_Salary = @minSalary, Max_Salary = @maxSalary WHERE Id = @id";

        try
        {
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@title", title));
            command.Parameters.Add(new SqlParameter("@minSalary", minSalary));
            command.Parameters.Add(new SqlParameter("@maxSalary", maxSalary));

            connection.Open(); // Membuka koneksi.

            using var transaction = connection.BeginTransaction();
        
            try
            {
                command.Transaction = transaction; // Menetapkan object transaksi dengan object perintah SQL.
            
                var result = command.ExecuteNonQuery(); 
                
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

    public string Delete(string id)
    {
        using var connection = Provider.GetConnection();
        using var command = Provider.GetCommand();

        command.Connection = connection;
        command.CommandText = "DELETE FROM jobs WHERE Id = @id";

        try
        {
            command.Parameters.Add(new SqlParameter("@id", id));

            connection.Open(); // Membuka koneksi.
        
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction; // Menetapkan object transaksi dengan object perintah SQL.

                var result = command.ExecuteNonQuery(); 
                
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