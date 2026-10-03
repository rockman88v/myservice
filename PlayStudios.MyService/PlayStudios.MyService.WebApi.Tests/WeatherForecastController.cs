using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PlayStudios.MyService.WebApi.Controllers;
using Xunit;

namespace PlayStudios.MyService.WebApi.Tests
{
  public class WeatherForecastControllerTests
  {
    [Fact]
    public void ShouldReturnAForecast()
    {
      var controller = new WeatherForecastController(NullLogger<WeatherForecastController>.Instance);
      Assert.NotEmpty(controller.Get());
    }
  }
}
