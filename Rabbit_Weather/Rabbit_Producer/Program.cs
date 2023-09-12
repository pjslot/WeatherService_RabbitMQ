using System.Net.Http;
using System.Net;
using System;
using System.Text.Json;
//поставлены NuGeеt: Serilog, Serilog.Sinks.Console, Serilog.Extensions.Hosting, Serilog.Sinks.File
using Serilog;
using Serilog.Core;

namespace Rabbit_Weather
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//инициализация логгера
			Log.Logger = new LoggerConfiguration()
							.WriteTo.Console()
							.MinimumLevel.Debug()
							.CreateLogger();

			//конфиг запроса
			float latitude = 52.52F;
			float longitude = 13.41F;

			Location loc = new Location(latitude, longitude, true) ;

		    InfoGrabber test = new InfoGrabber();
			var z = test.ReturnInfo(loc);

			Log.Debug("Weather requested for coordinates: {lat} : {lon}", latitude, longitude);

			Console.WriteLine("Printing from main block:");
			Console.WriteLine(z.ToString());
			Console.WriteLine("DESERIALIZING:");

		

			Forecast? forecast= JsonSerializer.Deserialize<Forecast>(z);

			//json test
			Console.WriteLine(forecast.longitude);
			Console.WriteLine(forecast.current_weather.temperature);
			Console.WriteLine(forecast.current_weather.time);
			Console.WriteLine(forecast.generationtime_ms);

			Log.Information("Request finished");
		}
	}
}