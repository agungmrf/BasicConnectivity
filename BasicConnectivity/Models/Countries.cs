using System.Data.SqlClient;

namespace BasicConnectivity.Models
{
    public class Countries
    {
        public string Id { get; set; }
        public string CountryName { get; set; }
        public int? RegionId { get; set; }
        
        public override string ToString()
        {
            return $"{Id} - {CountryName} - {RegionId}";
        }

        public List<Countries> GetAll()
        {
            var countries = new List<Countries>();
            
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM countries";

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        countries.Add(new Countries
                        {
                            Id = reader.GetString(0),
                            CountryName = reader.GetString(1),
                            RegionId = reader.GetInt32(2)
                        });
                    }

                    reader.Close();
                    connection.Close();

                    return countries;
                }

                reader.Close();
                connection.Close();

                return new List<Countries>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Countries>();
            }
        }

        public Countries GetById(string countryId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM countries WHERE id = @countryId";

            try
            {
                command.Parameters.Add(new SqlParameter("@countryId", countryId));

                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Countries country = null;

                    while (reader.Read())
                    {
                        country = new Countries
                        {
                            Id = reader.GetString(0),
                            CountryName = reader.GetString(1),
                            RegionId = reader.GetInt32(2)
                        };
                    }
                    reader.Close();
                    connection.Close();

                    return country;
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

        public string Insert(Countries countries)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "INSERT INTO countries (id, name, region_id) VALUES (@id, @countryName, @regionId)";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", countries.Id));
                command.Parameters.Add(new SqlParameter("@countryName", countries.CountryName));
                command.Parameters.Add(new SqlParameter("@regionId", countries.RegionId));

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
                    return $"Error Transaction: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public string Update(string id, string countryName, int regionId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "UPDATE countries SET name = @countryName, region_id = @regionId WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@countryId", id));
                command.Parameters.Add(new SqlParameter("@countryName", countryName));
                command.Parameters.Add(new SqlParameter("@regionId", regionId));

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
            command.CommandText = "DELETE FROM countries WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@countryId", id));

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
