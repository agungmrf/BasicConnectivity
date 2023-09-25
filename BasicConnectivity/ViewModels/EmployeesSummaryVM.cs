namespace BasicConnectivity.ViewModels;

public class EmployeesSummaryVM
{
    public string Department_Name { get; set; }
    public int Total_Employee { get; set; }
    public decimal Min_Salary { get; set; }
    public decimal Max_Salary { get; set; }
    public decimal Average_Salary { get; set; }
    
    public override string ToString()
    {
        return $"{Department_Name} - {Total_Employee} - {Min_Salary} - {Max_Salary} - {Average_Salary}";
    }

}