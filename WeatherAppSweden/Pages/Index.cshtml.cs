using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherAppSweden.Models;
using WeatherAppSweden.Services;

namespace WeatherAppSweden.Pages;

public class IndexModel : PageModel
{
    private readonly WeatherService _weatherService;
    public IndexModel(WeatherService weatherService) => _weatherService = weatherService;

    public record CityWeather(
        string City,
        double Temperature,
        double Precipitation,
        double WindSpeed,
        double FeelsLike,      
        string Comment);

    public List<CityWeather> WeatherList { get; set; } = new();

    public async Task OnGetAsync()
    {
        var tasks = Cities.SwedishCities.Select(c =>
            _weatherService.GetCurrentWeatherAsync(c.Name, c.Latitude, c.Longitude));

        var raw = await Task.WhenAll(tasks);

        WeatherList = raw
            .Where(w => w != null && w.Temperature > -40)
            .Select(w => new CityWeather(
                City: w!.City,
                Temperature: Math.Round(w.Temperature, 1),
                Precipitation: w.Precipitation,
                WindSpeed: w.WindSpeed,

                
                FeelsLike: Math.Round(
                    w.Temperature

                    
                    + (w.Temperature <= 10 && w.WindSpeed > 5
                        ? -0.45 * Math.Pow(w.WindSpeed, 0.16) * (10 - w.Temperature) / 5
                        : 0)

                    
                    + (w.Precipitation > 0 ? -3 : 0)

                    
                    + (w.Precipitation == 0
                        ? w.Temperature switch { > 20 => 5, > 10 => 4, > 0 => 3, _ => 2 }
                        : 0),

                    1),

                Comment: Math.Round(w.Temperature +
                    (w.Temperature <= 10 && w.WindSpeed > 5 ? -0.45 * Math.Pow(w.WindSpeed, 0.16) * (10 - w.Temperature) / 5 : 0) +
                    (w.Precipitation > 0 ? -3 : 0) +
                    (w.Precipitation == 0 ? w.Temperature switch { > 20 => 5, > 10 => 4, > 0 => 3, _ => 2 } : 0), 1) switch
                {
                    >= 25 => "Sommarvärme – shorts och t-shirt!",
                    >= 20 => "Perfekt väder – njut ute!",
                    >= 15 => "Riktigt skönt – ta en promenad",
                    >= 10 => "Friskt och fint",
                    >= 5 => "Kyligt men okej",
                    >= 0 => "Kallt – jacka på!",
                    >= -10 => "Ruggigt – vind och kyla biter",
                    _ => "Arktiskt – stanna inne!"
                }
            ))
            .OrderByDescending(x => x.FeelsLike)     
            .ToList();
    }
}