using System.Net.Http;
using System.Net;
using System;
using System.Text.Json;


namespace Rabbit_Weather
{
	internal class Program
	{
		static void Main(string[] args)
		{

			Location loc = new Location(52.52F, 13.41F, true) ;

		    InfoGrabber test = new InfoGrabber();
			var z = test.ReturnInfo(loc);

			Console.WriteLine("Printing from main block:");
			Console.WriteLine(z.ToString());
			Console.WriteLine("DESERIALIZING:");

			Forecast? forecast= JsonSerializer.Deserialize<Forecast>(z);

			//json test
			Console.WriteLine(forecast.longitude);
			Console.WriteLine(forecast.current_weather.temperature);
			Console.WriteLine(forecast.current_weather.time);
			Console.WriteLine(forecast.generationtime_ms);
		}

	}
}