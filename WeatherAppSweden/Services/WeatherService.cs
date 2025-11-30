using System.Globalization;
using System.Text.Json;
using WeatherAppSweden.Models;

namespace WeatherAppSweden.Services;

public class WeatherService
{
    private readonly HttpClient _http;

    public WeatherService(HttpClient http)
    {
        _http = http;
    }

    public async Task<WeatherData?> GetCurrentWeatherAsync(string city, double lat, double lon)
    {
        
        var url = $"https://api.open-meteo.com/v1/forecast" +
                  $"?latitude={lat.ToString(CultureInfo.InvariantCulture)}" +
                  $"&longitude={lon.ToString(CultureInfo.InvariantCulture)}" +
                  $"&current=temperature_2m,precipitation,windspeed_10m&timezone=Europe/Berlin";

        try
        {
            var json = await _http.GetStringAsync(url);
            var doc = JsonDocument.Parse(json);
            var current = doc.RootElement.GetProperty("current");

            return new WeatherData
            {
                City = city,
                Temperature = Math.Round(current.GetProperty("temperature_2m").GetDouble(), 1),
                Precipitation = current.GetProperty("precipitation").GetDouble(),
                WindSpeed = current.GetProperty("windspeed_10m").GetDouble(),
                Time = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fel för {city}: {ex.Message}");
            return new WeatherData { City = city + " (fel)", Temperature = -99 };
        }
    }
}
 