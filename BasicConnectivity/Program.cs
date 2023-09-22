using BasicConnectivity.Controllers;
using BasicConnectivity.Views;

namespace BasicConnectivity
{
    public class Program
    {
        private static void Main()
        {
            var choice = true;
            while (choice)
            {
                Console.WriteLine("HR Database System:");
                Console.WriteLine("1. Region CRUD");
                Console.WriteLine("2. View Countries");
                Console.WriteLine("3. View Locations");
                Console.WriteLine("4. View Jobs");
                Console.WriteLine("5. View Employees");
                Console.WriteLine("6. View Departments");
                Console.WriteLine("7. View Histories");
                Console.WriteLine("8. Join Table Employees");
                Console.WriteLine("9. Employees Summary");
                Console.WriteLine("10. Exit");
                Console.Write("Enter your choice: ");
                var input = Console.ReadLine();
                choice = Menu(input);
            }
        }

        public static bool Menu(string input)
        {
            switch (input)
            {
                case "1":
                    RegionMenu();
                    break;
                case "2":
                    // Menampilkan daftar Countries.
                    var country = new Countries();
                    var countries = country.GetAll();
                    //GeneralView.List(countries, "countries");
                    break;
                case "3":
                    // Menampilkan daftar Locations.
                    var location = new Locations();
                    var locations = location.GetAll();
                    //GeneralView.List(locations, "locations");
                    break;
                case "4":
                    // Menampilkan daftar Jobs.
                    var jobs = new Jobs();
                    var allJobs = jobs.GetAll();
                    //GeneralView.List(allJobs, "jobs");
                    break;
                case "5":
                    // Menampilkan daftar Employees.
                    var employees = new Employees();
                    var allEmployees = employees.GetAll();
                    //GeneralView.List(allEmployees, "employees");
                    break;
                case "6":
                    // Menampilkan daftar Histories.
                    var histories = new Histories();
                    var allHistories = histories.GetAll();
                    //GeneralView.List(allHistories, "histories");
                    break;
                case "7":
                    // Menampilkan daftar Departments.
                    var departments = new Departments();
                    var allDepartments = departments.GetAll();
                    //GeneralView.List(allDepartments, "departments");
                    break;
                case "8":
                    // Membuat object-object untuk operasi JOIN.
                    var employees1 = new Employees();
                    var departments1 = new Departments();
                    var locations1 = new Locations();
                    var countries1 = new Countries();
                    var region1 = new Region();

                    // Mengambil semua data yang dibutuhkan dari berbagai tabel.
                    var getEmployees = employees1.GetAll();
                    var getDepartments = departments1.GetAll();
                    var getLocations = locations1.GetAll();
                    var getCountries = countries1.GetAll();
                    var getRegion = region1.GetAll();

                    // Melakukan operasi JOIN dan SELECT ke object EmployeesVM
                    var resultJoin = (from e in getEmployees
                        join d in getDepartments on e.DepartmentId equals d.Id
                        join l in getLocations on d.LocationId equals l.Id
                        join c in getCountries on l.CountryId equals c.Id
                        join r in getRegion on c.RegionId equals r.Id
                        select new EmployeesVM
                        {
                            Id = e.Id,
                            Full_Name = e.FirstName + " " + e.LastName,
                            Email = e.Email,
                            Phone_Number = e.PhoneNumber,
                            Salary = e.Salary,
                            Department_Name = d.DepartmentName,
                            Street_Address = l.StreetAddress,
                            Country_Name = c.CountryName,
                            Region_Name = r.Name
                        }).ToList();

                    // Menampilkan hasil ke layar.
                    foreach (var item in resultJoin)
                    {
                        Console.WriteLine(
                            $"{item.Id} - {item.Full_Name} - {item.Email} - {item.Phone_Number} - {item.Salary} - {item.Department_Name} - {item.Street_Address} - {item.Country_Name} - {item.Region_Name}");
                    }

                    break;
                case "9":
                    // Membuat object-object untuk operasi JOIN dan GROUP BY.
                    var employees2 = new Employees();
                    var departments2 = new Departments();

                    // Mengambil semua data yang dibutuhkan dari berbagai tabel.
                    var getEmployees1 = employees2.GetAll();
                    var getDepartments1 = departments2.GetAll();

                    // Melakukan operasi JOIN, GROUP BY, dan SELECT ke object EmployeesSummaryVM.
                    var resultJoin1 = (from e1 in getEmployees1
                        join d1 in getDepartments1 on e1.DepartmentId equals d1.Id
                        group e1 by d1.DepartmentName
                        into departmentGroup
                        where departmentGroup.Count() > 3
                        select new EmployeesSummaryVM
                        {
                            Department_Name = departmentGroup.Key,
                            Total_Employee = departmentGroup.Count(),
                            Min_Salary = departmentGroup.Min(e => e.Salary),
                            Max_Salary = departmentGroup.Max(e => e.Salary),
                            Average_Salary = (decimal)departmentGroup.Average(e => e.Salary)
                        }).ToList();

                    // Menampilkan hasil ke layar.
                    foreach (var item in resultJoin1)
                    {
                        Console.WriteLine(
                            $"Department Name: {item.Department_Name} - Total Employees: {item.Total_Employee} - Min Salary: {item.Min_Salary} - Max Salary: {item.Max_Salary} - Average Salary: {item.Average_Salary}");
                    }

                    break;
                case "10":
                    return false;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

            return true;
        }

        public static void RegionMenu()
        {
            var region = new Region();
            var regionView = new RegionView();

            var regionController = new RegionController(region, regionView);

            var isLoop = true;
            while (isLoop)
            {
                Console.WriteLine("1. List all regions");
                Console.WriteLine("2. Insert new regions");
                Console.WriteLine("3. Update regions");
                Console.WriteLine("4. Delete regions");
                Console.WriteLine("10. Back");
                Console.Write("Enter your choice: ");
                var input1 = Console.ReadLine();
                switch (input1)
                {
                    case "1":
                        regionController.GetAll();
                        break;
                    case "2":
                        regionController.Insert();
                        break;
                    case "3":
                        regionController.Update();
                        break;
                    case "10":
                        isLoop = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}
