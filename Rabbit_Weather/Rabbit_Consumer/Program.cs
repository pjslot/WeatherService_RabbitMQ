// Kabluchkov DS (c) 2023
// firstRun info:
// перед первым запуском раскомментироватьDatabase.EnsureDeleted(); //Database.EnsureCreated(); в EFWorker, после закомменитировать обратно.

using Serilog;
using Serilog.Core;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using Serilog.Sinks.SystemConsole.Themes;

namespace Rabbit_Consumer
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//таймер
			int timer = 5000;

			//инициализация логгера
			Log.Logger = new LoggerConfiguration()
							.WriteTo.Console()
							.MinimumLevel.Debug()
							.CreateLogger();

			List<string> list = new List<string>();

			//зацикливание
			while (true)
			{
				try
				{
					list = Reader.ReadMessage();
					Log.Information("City list grabbing from Rabbit...");
					if (list.Count == 0) Log.Information("Queue work finished or queue is empty.");
					else				
						foreach (string l in list.ToList())
						{
						Log.Information($"City {l} grabbed. Deserialising...", l);
						//десериализуем
						ForecastRabbit? forecast = JsonSerializer.Deserialize<ForecastRabbit>(l);
						//ищем город в текущей базе и решаем - добавлять новый или обновлять имеющийся
						Log.Information("Pushing to DB worker...");
						EFWorker.CityEditOrCreate(forecast.cityName, forecast.temperature);					
						}										
				}
				catch (Exception ex)
				{
					Log.Error(ex.Message);
				}

				Log.Information($"Timer sleep: {timer}", timer);
			Thread.Sleep(timer);				
			}
			//конец зацикливания
		}
	}
}