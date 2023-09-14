using Serilog;
using System.Reflection;

namespace Rabbit_Consumer
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");

			while (true)
			{

				try
				{
					if (Reader.ReadMessage() == null) Console.WriteLine("Разбор очереди окончен или очередь пуста.");

					//Log.Information($"New city {allCities[i].cityName} info sent to RabbutMQ succesfully.");
				}
				catch (Exception ex)
				{
					Log.Error(ex.Message);
				}

				Thread.Sleep(1000);
				
			}
		}
	}
}