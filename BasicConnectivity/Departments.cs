using System.Data.SqlClient;

namespace BasicConnectivity
{
    public class Departments
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int LocationId { get; set; }
        public int ManagerId { get; set; }

        public override string ToString()
        {
            return $"{Id} - {DepartmentName} - {LocationId} - {ManagerId}";
        }
        
        public List<Departments> GetAll()
        {
            var departments = new List<Departments>();

            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM departments";

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        departments.Add(new Departments
                        {
                            Id = reader.GetInt32(0),
                            DepartmentName = reader.GetString(1),
                            LocationId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            ManagerId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                        });
                    }

                    reader.Close();
                    connection.Close();

                    return departments;
                }

                reader.Close();
                connection.Close();

                return new List<Departments>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Departments>();
            }
        }

        public Departments GetById(int id)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM departments WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Departments department = null;

                    while (reader.Read())
                    {
                        department = new Departments
                        {
                            Id = reader.GetInt32(0),
                            DepartmentName = reader.GetString(1),
                            LocationId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            ManagerId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                        };
                    }
                    reader.Close();
                    connection.Close();

                    return department;
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

        public string Insert(string departmentName, int locationId, int managerId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "INSERT INTO departments (name, location_id, manager_id) VALUES (@departmentName, @locationId, @managerId)";

            try
            {
                command.Parameters.Add(new SqlParameter("@departmentName", departmentName));
                command.Parameters.Add(new SqlParameter("@locationId", locationId));
                command.Parameters.Add(new SqlParameter("@managerId", managerId));

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

        public string Update(int id, string departmentName, int locationId, int managerId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "UPDATE departments SET name = @departmentName, location_id = @locationId, manager_id = @managerId WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@departmentName", departmentName));
                command.Parameters.Add(new SqlParameter("@locationId", locationId));
                command.Parameters.Add(new SqlParameter("@managerId", managerId));

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

        public string Delete(int id)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "DELETE FROM departments WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", Id));

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
