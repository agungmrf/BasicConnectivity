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
                Console.WriteLine("1. View Region");
                Console.WriteLine("2. View Countries");
                Console.WriteLine("3. View Locations");
                Console.WriteLine("4. View Jobs");
                Console.WriteLine("5. View Employees");
                Console.WriteLine("6. View Histories");
                Console.WriteLine("7. Exit");
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
                    var region = new Region();
                    var regions = region.GetAll();
                    GeneralMenu.List(regions, "regions");
                    break;
                case "2":
                    var country = new Countries();
                    var countries = country.GetAll();
                    GeneralMenu.List(countries, "countries");
                    break;
                case "3":
                    var location = new Locations();
                    var locations = location.GetAll();
                    GeneralMenu.List(locations, "locations");
                    break;
                case "4":
                    var jobs = new Jobs();
                    var allJobs = jobs.GetAll();
                    GeneralMenu.List(allJobs, "jobs");
                    break;
                case "5":
                    var employees = new Employees();
                    var allEmployees = employees.GetAll();
                    GeneralMenu.List(allEmployees, "employees");
                    break;
                case "6":
                    var histories = new Histories();
                    var allHistories = histories.GetAll();
                    GeneralMenu.List(allHistories, "histories");
                    break;
                case "7":
                    return false;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
            return true;
        }
    }
}
