using System.Net.Http;
using System.Net;
using System;
using System.Linq;
using System.Text.Json;
//поставлены NuGeеt: Serilog, Serilog.Sinks.Console, Serilog.Extensions.Hosting, Serilog.Sinks.File
using Serilog;
using Serilog.Core;
using Rabbit_Producer;
using Serilog.Sinks.SystemConsole.Themes;
//поставлены NuGeеt: Microsoft.EntityFrameworkCore Microsoft.EntityFrameworkCore.SqlServer 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;


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
			Location loc = new Location("Moscow", 55.45F, 37.36F, true) ;
			Location loc2 = new Location("Saint-Petersburg", 59.93F, 30.31F, true);
			Location loc3= new Location("Omsk", 54.58F, 73.23F, true);
			Location loc4 = new Location("Chelyabinsk", 55.09F, 61.24F, true);
			Location loc5 = new Location("Taganrog", 47.14F, 38.54F, true);

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
		//	 while (true)
		//		{

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

			//запустить только раз при первом запуске, потом закомментировать.
			//EFWorker.DBPushData();

			//		Thread.Sleep(timeout);
			//	Console.Clear();

			//	} 



			Console.WriteLine(EFWorker.CityTempChecker("Taganrog", 995));














			//тест асинхронности ручных запросов
			//Task.WaitAll(InfoGrabber.PrintInfo(loc),
			//	InfoGrabber.PrintInfo(loc2),
			//	InfoGrabber.PrintInfo(loc3),
			//	InfoGrabber.PrintInfo(loc4),
			//	InfoGrabber.PrintInfo(loc5));

		}
	}
}