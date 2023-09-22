namespace BasicConnectivity.Views;

public class RegionView : GeneralView
{
    public string InsertInput()
    {
        Console.WriteLine("Insert region name");
        return Console.ReadLine();
    }
}