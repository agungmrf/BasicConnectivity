using BasicConnectivity.Models;
using BasicConnectivity.Views;

namespace BasicConnectivity.Controllers;

public class CountriesController
{
    private Countries _countries;
    private CountriesView _countriesView;

    public CountriesController(CountriesView countriesView, Countries countries)
    {
        _countriesView = countriesView;
        _countries = countries;
    }
    public void GetAll()
    {
        var results = _countries.GetAll();

        if (results.Count == 0)
        {
            Console.WriteLine("No countries found");
        }
        else
        {
            _countriesView.List(results, "countries");
        }
    }

    public void Insert()
    {
        var country = _countriesView.InsertCountry();

        if (country != null)
        {
            var result = _countries.Insert(country);
            _countriesView.Transaction(result);
        }
        else
        {
            Console.WriteLine("Invalid input for country");
        }
    }

    public void Update()
    {
        
    }

    public void Delete()
    {
        int idToDelete = _countriesView.DeleteCountry();

        if (idToDelete != -1)
        {
            var result = _countries.Delete(idToDelete.ToString());
            _countriesView.Transaction(result);
        }
    }
}