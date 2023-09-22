namespace BasicConnectivity;

public class GeneralView
{
    // Method untuk menampilkan hasil dari operasi "Get" (mengambil data) dalam daftar.
    // T mewakili tipe data yang berbeda, dan items yaitu daftar object dari tipe data.
    public void List<T>(List<T> items, string title)
    {
        Console.WriteLine($"List of {title}");
        Console.WriteLine("---------------");
        // Mengulang melalui setiap item dalam daftar.
        foreach (var item in items)
        {
            Console.WriteLine(item.ToString());
        }
    }
    
    public void Transaction(string result)
    {
        int.TryParse(result, out int res);
        if (res > 0)
        {
            Console.WriteLine("Transaction completed successfully");
        }
        else
        {
            Console.WriteLine("Transaction failed");
            Console.WriteLine(result);
        }
    }
}