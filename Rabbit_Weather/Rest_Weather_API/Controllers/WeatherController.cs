using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rest_Weather_API.Controllers;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Rest_Weather_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WeatherController : ControllerBase
	{
		//GET
		//правила пользования - например: https://localhost:7234/api/weather/59.93/30.31
		[HttpGet("{lat}/{lon}")]
		public string Get(float lat, float lon)
		{		
			return $"Temp in (lat:{lat.ToString()}-lon:{lon.ToString()}) is C: "+GiveMeTemp(ReturnInfo(lat, lon));
		}

		public static string GiveMeTemp(string forecast)
		{
			Forecast forecastDeserialized = JsonSerializer.Deserialize<Forecast>(forecast);
			return forecastDeserialized.current_weather.temperature.ToString();
		}

		public static string ReturnInfo(float lat, float lon)
		{
			var wb = new WebClient();
			try
			{
				string _lat = lat.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
				string _lon = lon.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
				var answer = wb.DownloadString($"https://api.open-meteo.com/v1/forecast?latitude={_lat}&longitude={_lon}&current_weather=true");
				return answer;
			}
			catch (Exception e)
			{
				Console.WriteLine($"ERROR!: {e.Message}");
			}
			finally
			{
				wb.Dispose();
			}
			return null;
		}

	}
}
