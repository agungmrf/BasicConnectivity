using BasicConnectivity.Models;

namespace BasicConnectivity.Views;

public class RegionView : GeneralView
{
    public string InsertRegion()
    {
        Console.WriteLine("Insert region name");
        return Console.ReadLine();
    }

    public Region UpdateRegion()
    {
        Console.WriteLine("Update region id");
        var id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Insert region name");
        var name = Console.ReadLine();

        return new Region
        {
            Id = id,
            Name = name
        };
    }
    
    public int DeleteRegion()
    {
        Console.WriteLine("Delete Region (Enter Region ID): ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            return id;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid ID.");
            return -1; // Mengembalikan nilai yang menunjukkan ID tidak valid.
        }
    }
}