namespace WeatherAppSweden.Services;

public record City(string Name, double Latitude, double Longitude);

public static class Cities
{
    public static readonly List<City> SwedishCities = new()
    {
        new("Stockholm", 59.3293, 18.0686),
        new("Göteborg", 57.7089, 11.9746),
        new("Malmö", 55.6050, 13.0038),
        new("Uppsala", 59.8586, 17.6389),
        new("Västerås", 59.6099, 16.5448),
        new("Örebro", 59.2753, 15.2134),
        new("Linköping", 58.4108, 15.6214),
        new("Helsingborg", 56.0467, 12.6944),
        new("Jönköping", 57.7815, 14.1770),
        new("Norrköping", 58.5877, 16.1924)
    };
}
 