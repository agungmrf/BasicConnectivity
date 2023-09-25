using System.Data.SqlClient;

namespace BasicConnectivity.Models
{
    public class Histories
    {
        public DateTime StartDate { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? EndDate { get; set; }
        public int DepartmentId { get; set; }
        public string JobId { get; set; }
        
        public override string ToString()
        {
            return $"{StartDate} - {EmployeeId} - {EndDate} - {DepartmentId} - {JobId}";
        }

        public List<Histories> GetAll()
        {
            var histories = new List<Histories>();

            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM histories";

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        histories.Add(new Histories
                        {
                            StartDate = reader.GetDateTime(0),
                            EmployeeId = reader.GetInt32(1),
                            EndDate = reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2),
                            DepartmentId = reader.GetInt32(3),
                            JobId = reader.GetString(4)
                        });
                    }

                    reader.Close();
                    connection.Close();

                    return histories;
                }

                reader.Close();
                connection.Close();

                return new List<Histories>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Histories>();
            }
        }

        public Histories GetById(DateTime startDate, int employeeId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM histories WHERE start_date = @startDate AND employee_id = @employeeId ";

            try
            {
                command.Parameters.Add(new SqlParameter("@startDate", startDate));
                command.Parameters.Add(new SqlParameter("@employeeId", employeeId));

                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Histories history = null;

                    while (reader.Read())
                    {
                        history = new Histories
                        {
                            StartDate = reader.GetDateTime(0),
                            EmployeeId = reader.GetInt32(1),
                            EndDate = reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2),
                            DepartmentId = reader.GetInt32(3),
                            JobId = reader.GetString(4)
                        };
                    }

                    reader.Close();
                    connection.Close();

                    return history;
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

        public string Insert( DateTime startDate, int employeeId, DateTime? endDate, int departmentId, string jobId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "INSERT INTO histories (StartDate, EmployeeId, EndDate, DepartmentId, JobId) VALUES (@startDate, @employeeId, @endDate, @departmentId, @jobId)";

            try
            {
                command.Parameters.Add(new SqlParameter("@startDate", startDate));
                command.Parameters.Add(new SqlParameter("@employeeId", employeeId));
                command.Parameters.Add(new SqlParameter("@endDate", endDate.HasValue ? endDate : (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@departmentId", departmentId));
                command.Parameters.Add(new SqlParameter("@jobId", jobId));

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

        public string Update(DateTime startDate, int employeeId, DateTime? endDate, int departmentId, string jobId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "UPDATE histories SET EndDate = @endDate, DepartmentId = @departmentId, JobId = @jobId WHERE StartDate = @startDate AND EmployeeId = @employeeId";

            try
            {
                command.Parameters.Add(new SqlParameter("@startDate", startDate));
                command.Parameters.Add(new SqlParameter("@employeeId", employeeId));
                command.Parameters.Add(new SqlParameter("@endDate", endDate.HasValue ? endDate : (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@departmentId", departmentId));
                command.Parameters.Add(new SqlParameter("@jobId", jobId));

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

        public string Delete(int employeeId, DateTime startDate)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "DELETE FROM histories WHERE StartDate = @startDate AND EmployeeId = @employeeId";

            try
            {
                command.Parameters.Add(new SqlParameter("@startDate", startDate));
                command.Parameters.Add(new SqlParameter("@employeeId", employeeId));

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
