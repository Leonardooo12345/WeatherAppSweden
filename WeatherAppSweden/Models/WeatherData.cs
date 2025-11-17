namespace WeatherAppSweden.Models;

public class WeatherData
{
    public string City { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public double Precipitation { get; set; }
    public double WindSpeed { get; set; }
    public DateTime Time { get; set; }
}