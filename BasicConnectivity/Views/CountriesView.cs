using BasicConnectivity.Models;

namespace BasicConnectivity.Views;

public class CountriesView : GeneralView
{
    public Countries InsertCountry()
    {
        Console.Write("Enter country ID: ");
        var id = Console.ReadLine();

        Console.Write("Enter country name: ");
        var name = Console.ReadLine();

        Console.Write("Enter region ID: ");
        if (int.TryParse(Console.ReadLine(), out int regionId))
        {
            // Buat objek Countries baru dengan semua nilai yang diterima dari pengguna
            var country = new Countries
            {
                Id = id,
                CountryName = name,
                RegionId = regionId
            };

            return country;
        }
        else
        {
            Console.WriteLine("Invalid input for region ID");
            return null; // Mengembalikan null jika input region ID tidak valid
        }
    }

    public Countries UpdateCountry()
    {
        Console.Write("Enter the ID of the country to update: ");
        var id = Console.ReadLine();

        Console.Write("Enter new country name (or press Enter to keep it unchanged): ");
        var name = Console.ReadLine();

        Console.Write("Enter new region ID (or press Enter to keep it unchanged): ");
        var regionIdInput = Console.ReadLine();
        int? regionId = null; // Use int? here

        if (!string.IsNullOrWhiteSpace(regionIdInput) && int.TryParse(regionIdInput, out int parsedRegionId))
        {
            regionId = parsedRegionId;
        }

        // Create a new Countries object with values received from the user
        var updatedCountry = new Countries
        {
            Id = id,
            CountryName = !string.IsNullOrWhiteSpace(name) ? name : null,
            RegionId = regionId // Assign nullable regionId
        };

        return updatedCountry;
    }
    
    public int DeleteCountry()
    {
        Console.Write("Delete Countries (Enter Countries ID): ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            return id;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid ID.");
            return -1;
        }
    }
}