// Kabluchkov DS (c) 2023
// firstRun info:
// перед первым запуском раскомментировать EFWorker.DBPushData(); в Program, после закомменитировать обратно.
// перед первым запуском раскомментироватьDatabase.EnsureDeleted(); //Database.EnsureCreated(); в EFWorker, после закомменитировать обратно.

using System.Net.Http;
using System.Net;
using System;
using System.Linq;
using System.Text.Json;
//поставлены NuGet: Serilog, Serilog.Sinks.Console, Serilog.Extensions.Hosting, Serilog.Sinks.File
using Serilog;
using Serilog.Core;
using Rabbit_Producer;
using Serilog.Sinks.SystemConsole.Themes;
//поставлены NuGеt: Microsoft.EntityFrameworkCore Microsoft.EntityFrameworkCore.SqlServer 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//поставлен NuGet: RabbitMQ.Client
using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;

//добавлено
using Microsoft.Extensions.DependencyInjection;

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

			//тест кролика
			try
			{
				Sender.SendMessage();
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
			}
		
			Console.WriteLine("done?");
			Console.ReadLine();




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

		
			//получаем прогнозы
		//	 while (true)
		//		{

				//обрабатываем все таски перед импортом
				Task.WaitAll(AsyncCollector.Returner(allCities));

				List<string> collection = new List<string>(AsyncCollector.Returned());
				Log.Information("All Cities Forecast Grabbed To List");

				Console.WriteLine($"Timeout: {timeout}");

			//запустить только раз при первом запуске, потом закомментировать.
			//EFWorker.DBPushData();



			//	Thread.Sleep(timeout);
			//	Console.Clear();

			//	} 


			//едем по всей коллекции городов сграбленных с погодного сайта	
			int i = 1;

			foreach (string city in collection)
			{
				Log.Information($"Работаем с городом: {allCities[i].cityName}");

				//десериализуем прогноз
				Forecast forecast = JsonSerializer.Deserialize<Forecast>(city);

				//если температура изменилась
				//if (EFWorker.CityTempChecker(allCities[i].cityName, forecast.current_weather.temperature))
				
				//для показательности мониторим время генерации а не температуру
				if (EFWorker.CityTempChecker(allCities[i].cityName, forecast.generationtime_ms))
				{
					//если поправили базу успешно
					//	if (EFWorker.EditCityTemp(allCities[i].cityName, forecast.current_weather.temperature))
					if (EFWorker.EditCityTemp(allCities[i].cityName, forecast.generationtime_ms))
					{
						//тут дёргаем зайца, но ещё ещё нету.
					};
				}

				Log.Information($"Работа с городом: {allCities[i++].cityName} завершена.");
				Console.WriteLine("=======================================================");
		
			}

			














			//тест асинхронности ручных запросов
			//Task.WaitAll(InfoGrabber.PrintInfo(loc),
			//	InfoGrabber.PrintInfo(loc2),
			//	InfoGrabber.PrintInfo(loc3),
			//	InfoGrabber.PrintInfo(loc4),
			//	InfoGrabber.PrintInfo(loc5));



		}

		

		
		//подвязка сервисов рэббит
		//public void ConfigureServices(IServiceCollection services)
		//{
		//	services.AddScoped<Rabbit_Producer.RabbitMq.IRabbitMqService, Rabbit_Producer.RabbitMq.RabbitMqService>();
		//}
	}

	
}