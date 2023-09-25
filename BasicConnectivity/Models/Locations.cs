using System.Data.SqlClient;

namespace BasicConnectivity.Models
{
    public class Locations
    {
        // Properti yang merepresentasikan kolom-kolom dalam tabel 'Locations'
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string CountryId { get; set; }
        
        // Method untuk menggabungkan data properti menjadi sebuah string
        public override string ToString()
        {
            return $"{Id} - {StreetAddress} - {PostalCode} - {City} - {StateProvince} - {CountryId}";
        }
        
        public List<Locations> GetAll()
        {
            var locations = new List<Locations>();

            using var connection = Provider.GetConnection(); // Menggunakan kelas Provider untuk mendapatkan koneksi.
            using var command = Provider.GetCommand(); // Menggunakan kelas Provider untuk mendapatkan perintah SQL.

            // Menentukan koneksi dan perintah SQL
            command.Connection = connection;
            command.CommandText = "SELECT * FROM locations";

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    // Membaca data dan menambahkan ke daftar Locations
                    while (reader.Read())
                    {
                        locations.Add(new Locations
                        {
                            Id = reader.GetInt32(0),
                            StreetAddress = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            PostalCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            City = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            StateProvince = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            CountryId = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                        });
                    }

                    reader.Close();
                    connection.Close();

                    return locations;
                }

                reader.Close();
                connection.Close();

                return new List<Locations>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Locations>();
            }
        }

        public Locations GetById(int id)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM locations WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Locations location = null;

                    while (reader.Read())
                    {
                        location = new Locations
                        {
                            Id = reader.GetInt32(0),
                            StreetAddress = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            PostalCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            City = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            StateProvince = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            CountryId = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                        };
                    }
                    reader.Close();
                    connection.Close();

                    return location;
                }
                reader.Close();
                connection.Close();

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public string Insert(string streetAddress, string postalCode, string city, string stateProvince, string countryId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "INSERT INTO locations (street_address, postal_code, city, state_province, country_id) VALUES (@streetAddress, @postalCode, @city, @stateProvince, @countryId)";

            try
            {
                command.Parameters.Add(new SqlParameter("@streetAddress", streetAddress));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@stateProvince", stateProvince));
                command.Parameters.Add(new SqlParameter("@countryId", countryId));

                connection.Open();
                using var transaction = connection.BeginTransaction();
                try
                {
                    command.Transaction = transaction;

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

        public string Update(int id, string streetAddress, string postalCode, string city, string stateProvince, string countryId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "UPDATE locations SET street_address = @streetAddress, postal_code = @postalCode, city = @city, state_province = @stateProvince, country_id = @countryId WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@streetAddress", streetAddress));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@stateProvince", stateProvince));
                command.Parameters.Add(new SqlParameter("@countryId", countryId));

                connection.Open();

                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;

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

        public string Delete(int id)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "DELETE FROM locations WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();

                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;

                    var result = command.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();

                    return result.ToString();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return $"Error: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
