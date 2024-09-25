using FluentValidation;

namespace Application.Weather.DTOs
{
    public class AddWeatherForecastDTOValidator : AbstractValidator<WeatherForecastDTO>
    {
        public AddWeatherForecastDTOValidator()
        {
            RuleFor(u => u.Summary)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.");

            RuleFor(u => u.TemperatureC)
                .GreaterThanOrEqualTo(0)
                .WithMessage("'Temperature C must be greater or equal to 0'");

            RuleFor(u => u.Date)
                .NotNull()
                .Must(date => date >= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Date must be from today to 7 days later.");
        }
    }
}
