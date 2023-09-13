using System.Net.Http;
using System.Net;
using System;
using System.Text.Json;
//поставлены NuGeеt: Serilog, Serilog.Sinks.Console, Serilog.Extensions.Hosting, Serilog.Sinks.File
using Serilog;
using Serilog.Core;
using Rabbit_Producer;
using Serilog.Sinks.SystemConsole.Themes;

namespace Rabbit_Weather
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			//инициализация логгера
			Log.Logger = new LoggerConfiguration()
							.WriteTo.Console()
							.MinimumLevel.Debug()
							.CreateLogger();

			//конфиг запроса
			Location loc = new Location("One", 11.10F, 11.21F, true) ;
			Location loc2 = new Location("Two", 21.10F, 21.21F, true);
			Location loc3= new Location("Three", 31.10F, 31.21F, true);
			Location loc4 = new Location("Four", 41.10F, 41.21F, true);
			Location loc5 = new Location("Five", 51.10F, 51.21F, true);

			Dictionary<int, Location> allCities = new Dictionary<int, Location>();
		
			allCities.Add(1, loc);
			allCities.Add(2, loc2);
			allCities.Add(3, loc3);
			allCities.Add(4, loc4);
			allCities.Add(5, loc5);

			//таймаут запроса
			int timeout = 5000;

			ConsoleKeyInfo cki = new ConsoleKeyInfo();

			//получаем прогнозы
			 while (true)
				{

				//обрабатываем все таски перед импортом
				Task.WaitAll(AsyncCollector.Returner(allCities));

				List<string> collection = new List<string>(AsyncCollector.Returned());
				Log.Information("All Cities Forecast Grabbed To List");

				//десериализуем лист прогноза
				foreach (string s in collection)
				{
					Forecast forecast = JsonSerializer.Deserialize<Forecast>(s);
					Log.Debug("Weather requested for coordinates: {lat} : {lon}", forecast.latitude, forecast.longitude);
					Console.WriteLine(forecast.current_weather.temperature);
					Console.WriteLine(forecast.generationtime_ms);
				}
				Log.Information("Request finished");

				Console.Write($"Timeout: {timeout}");
				Thread.Sleep(timeout);
				Console.Clear();

			} 

			//тест асинхронности ручных запросов
			//Task.WaitAll(InfoGrabber.PrintInfo(loc),
			//	InfoGrabber.PrintInfo(loc2),
			//	InfoGrabber.PrintInfo(loc3),
			//	InfoGrabber.PrintInfo(loc4),
			//	InfoGrabber.PrintInfo(loc5));

		}
	}
}