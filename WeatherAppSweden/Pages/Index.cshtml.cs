using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherAppSweden.Models;
using WeatherAppSweden.Services;

namespace WeatherAppSweden.Pages;

public class IndexModel : PageModel
{
    private readonly WeatherService _weatherService;

    public IndexModel(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    
    public record CityResult(
        string City,
        double Temperature,
        double Rain,
        double Wind,
        double OutdoorScore,
        string Comment);

    public List<CityResult> WeatherList { get; set; } = new();

    public async Task OnGetAsync()
    {
        var tasks = Cities.SwedishCities.Select(c =>
            _weatherService.GetCurrentWeatherAsync(c.Name, c.Latitude, c.Longitude));

        var rawData = await Task.WhenAll(tasks);

        
        WeatherList = rawData
            .Where(w => w != null && w.Temperature > -40)
            .Select(w => new CityResult(
                City: w!.City,
                Temperature: Math.Round(w.Temperature, 1),
                Rain: w.Precipitation,
                Wind: w.WindSpeed,
                OutdoorScore: Math.Round(w.Temperature - w.Precipitation * 3 - w.WindSpeed * 0.5, 1),
                Comment: w.Temperature switch
                {
                    > 15 => "Perfekt uteväder!",
                    > 5 => "Ganska okej",
                    > 0 => "Kallt men okej",
                    _ => "Kallt som fan – stanna inne!"
                }
            ))
            .OrderByDescending(x => x.OutdoorScore)
            .ToList();
    }
}