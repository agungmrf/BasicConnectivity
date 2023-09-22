using BasicConnectivity.Views;

namespace BasicConnectivity.Controllers;

public class RegionController
{
    // Variabel untuk menyimpan instance Region dan GeneralView yang akan diinjeksi.
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
        var input = "";
        var isTrue = true;
        while (isTrue)
        {
            try
            {
                input = _regionView.InsertInput();
                isTrue = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        var results = _region.Insert(new Region
        {
            Id = 0,
            Name = input
        });

        _regionView.Transaction(results);
    }
}