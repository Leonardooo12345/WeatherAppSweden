using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherAppSweden.Models;
using WeatherAppSweden.Services;

namespace WeatherAppSweden.Pages;

public class IndexModel : PageModel
{
    private readonly WeatherService _weatherService;

    public IndexModel(WeatherService weatherService) => _weatherService = weatherService;

    public List<WeatherData> WeatherList { get; set; } = new();

    public async Task OnGetAsync()
    {
        var tasks = Cities.SwedishCities.Select(c =>
            _weatherService.GetCurrentWeatherAsync(c.Name, c.Latitude, c.Longitude));

        var results = await Task.WhenAll(tasks);

        WeatherList = results
            .Where(w => w is not null && w.Temperature > -50)
            .OrderByDescending(w => w.Temperature)
            .ToList();
    }
}