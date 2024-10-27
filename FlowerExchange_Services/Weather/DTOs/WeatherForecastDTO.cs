namespace Application.Weather.DTOs
{
    public class WeatherForecastDTO
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public string? Summary { get; set; }

    }
}
