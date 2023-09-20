using System.Data.SqlClient;

namespace BasicConnectivity
{
    public class Employees
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public int Salary { get; set; }
        public decimal CommissionPct { get; set; }
        public int ManagerId { get; set; }
        public string JobId { get; set; }
        public int DepartmentId { get; set; }
        
        public override string ToString()
        {
            return $"{Id} - {FirstName} - {LastName} - {Email} - {PhoneNumber} - {HireDate} - {Salary} - {CommissionPct} - {ManagerId} - {JobId} - {DepartmentId}";
        }
        
        public List<Employees> GetAll()
        {
            var employees = new List<Employees>();

            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM employees";

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employees
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            PhoneNumber = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            HireDate = reader.GetDateTime(5),
                            Salary = reader.GetInt32(6),
                            CommissionPct = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                            ManagerId = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                            JobId = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                            DepartmentId = reader.IsDBNull(10) ? 0 : reader.GetInt32(10)
                        });
                    }

                    reader.Close();
                    connection.Close();

                    return employees;
                }

                reader.Close();
                connection.Close();

                return new List<Employees>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Employees>();
            }
        }

        public Employees GetById(int id)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM employees WHERE Id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));

                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Employees employee = null;

                    while (reader.Read())
                    {
                        employee = new Employees
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            PhoneNumber = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            HireDate = reader.GetDateTime(5),
                            Salary = reader.GetInt32(6),
                            CommissionPct = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                            ManagerId = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                            JobId = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                            DepartmentId = reader.IsDBNull(10) ? 0 : reader.GetInt32(10)
                        };
                    }

                    reader.Close();
                    connection.Close();

                    return employee;
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

        public string Insert(string firstName, string lastName, string email, string phoneNumber, DateTime hireDate, int salary, decimal commissionPct, int managerId, string jobId, int departmentId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "INSERT INTO employees (first_name, last_name, email, phone_number, hire_date, salary, commission_pct, manager_id, job_id, department_id) VALUES (@firstName, @lastName, @email, @phoneNumber, @hireDate, @salary, @commissionPct, @managerId, @jobId, @departmentId)";

            try
            {
                command.Parameters.Add(new SqlParameter("@firstName", firstName));
                command.Parameters.Add(new SqlParameter("@lastName", lastName));
                command.Parameters.Add(new SqlParameter("@email", email));
                command.Parameters.Add(new SqlParameter("@phoneNumber", phoneNumber));
                command.Parameters.Add(new SqlParameter("@hireDate", hireDate));
                command.Parameters.Add(new SqlParameter("@salary", salary));
                command.Parameters.Add(new SqlParameter("@commissionPct", commissionPct));
                command.Parameters.Add(new SqlParameter("@managerId", managerId));
                command.Parameters.Add(new SqlParameter("@jobId", jobId));
                command.Parameters.Add(new SqlParameter("@departmentId", departmentId));

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

        public string Update(int id, string firstName, string lastName, string email, string phoneNumber, DateTime hireDate, int salary, decimal commissionPct, int managerId, string jobId, int departmentId)
        {
            using var connection = Provider.GetConnection();
            using var command = Provider.GetCommand();

            command.Connection = connection;
            command.CommandText = "UPDATE employees SET first_name = @firstName, last_name = @lastName, email = @email, phone_number = @phoneNumber, hire_date = @hireDate, salary = @salary, commission_pct = @commissionPct, manager_id = @managerId, job_id = @jobId, department_id = @departmentId WHERE id = @id";

            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@firstName", firstName));
                command.Parameters.Add(new SqlParameter("@lastName", lastName));
                command.Parameters.Add(new SqlParameter("@email", email));
                command.Parameters.Add(new SqlParameter("@phoneNumber", phoneNumber));
                command.Parameters.Add(new SqlParameter("@hireDate", hireDate));
                command.Parameters.Add(new SqlParameter("@salary", salary));
                command.Parameters.Add(new SqlParameter("@commissionPct", commissionPct));
                command.Parameters.Add(new SqlParameter("@managerId", managerId));
                command.Parameters.Add(new SqlParameter("@jobId", jobId));
                command.Parameters.Add(new SqlParameter("@departmentId", departmentId));

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
            command.CommandText = "DELETE FROM employees WHERE id = @id";

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
