namespace BasicConnectivity;

public class GeneralMenu
{
    public static void List<T>(List<T> items, string title)
    {
        Console.WriteLine($"List of {title}");
        Console.WriteLine("---------------");
        foreach (var item in items)
        {
            Console.WriteLine(item.ToString());
        }
    }
    
    
}