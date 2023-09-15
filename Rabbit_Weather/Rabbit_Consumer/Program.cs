using Rabbit_Producer;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Threading;

namespace Rabbit_Consumer
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");

			List<string> list = new List<string>();

			//зацикливание. пока приглушено.
			//while (true)
			//{
				try
				{
					list = Reader.ReadMessage();
					if (list.Count == 0) Console.WriteLine("Разбор очереди окончен или очередь пуста.");

					
						foreach (string l in list)
						{
							Console.WriteLine(l);
							Console.WriteLine("Вывели наружу, десериализуем");
						
						//десериализуем
						ForecastRabbit? forecast = JsonSerializer.Deserialize<ForecastRabbit>(l);

						//ищем город в текущей базе и решаем - добавлять новый или обновлять имеющицся
						Console.WriteLine(forecast?.cityName + forecast?.temperature);
						}					
					
				}
				catch (Exception ex)
				{
					Log.Error(ex.Message);
				}

				Thread.Sleep(5000);
				
			//}
			//конец зацикливания
		}
	}
}