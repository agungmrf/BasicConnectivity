using BasicConnectivity.Models;
using BasicConnectivity.Views;

namespace BasicConnectivity.Controllers;

public class RegionController
{
    // Variable untuk menyimpan instance Region dan GeneralView yang akan diinjeksi.
    private Region _region;
    private RegionView _regionView;

    // Konstruktor RegionController yang menerima injeksi Region dan GeneralView.
    public RegionController(Region region, RegionView regionView)
    {
        _region = region;
        _regionView = regionView;
    }

    // Metode GetAll untuk mengambil semua data Region.
    public void GetAll()
    {
        var results = _region.GetAll();
        // Memeriksa apakah hasilnya kosong.
        // Pembuatan condition harus dimulai dari dari kondisi false/error terlebih dahulu.
        if (!results.Any()) // Jika tidak ada data, menampilkan pesan "no data found".
        {
            Console.WriteLine("no data found");
        }
        else // Jika ada data, gunakan GeneralView untuk menampilkan data dalam bentuk daftar.
        {
            _regionView.List(results, "regions");
        }
    }

    public void Insert()
    {
        string input = "";
        var isTrue = true;
        while (isTrue)
        {
            try
            {
                input = _regionView.InsertRegion();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Region name cannot be empty");
                    continue;
                }
                isTrue = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        var result = _region.Insert(new Region {
            Id = 0,
            Name = input
        });
        
        _regionView.Transaction(result);
    }

    public void Update()
    {
        var region = new Region();
        var isTrue = true;
        while (isTrue)
        {
            try
            {
                region = _regionView.UpdateRegion();
                if (string.IsNullOrEmpty(region.Name))
                {
                    Console.WriteLine("Region name cannot be empty");
                    continue;
                }
                isTrue = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        var result = _region.Update(region.Id, region.Name);
        _regionView.Transaction(result);
    }
    
    public void Delete()
    {
        int idToDelete = _regionView.DeleteRegion();
            
        if (idToDelete != -1)
        {
            var result = _region.Delete(idToDelete);
            _regionView.Transaction(result);
        }
    }
}