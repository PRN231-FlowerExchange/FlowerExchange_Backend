using Application.Weather.DTOs;
using FluentValidation;


namespace Application.Weather.Commands.AddWeather
{
    public class AddWeatherCommandValidator : AbstractValidator<AddWeatherCommand>
    {
        public AddWeatherCommandValidator() {
            RuleFor(w => w.WeatherForecastDTO)
             .NotNull()
             .SetValidator(new AddWeatherForecastDTOValidator());
        }
    }
}
