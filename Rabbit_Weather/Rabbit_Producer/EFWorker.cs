using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Producer
{
	//сущность города
	public class City
	{
		[Key]
		public int Id { get; set; }
		public string? CityName { get; set; }
		public float Temperature { get; set; }
		public float GenerationTime { get; set; }
	}

	//КЛАСС КОНТЕКСТА
	public class ApplicationContext : DbContext
	{
		public DbSet<City> Cities { get; set; }
	
		public ApplicationContext()
		{
			//открывать только на первый запуск для инициализации базы, потом закрыть.
			//Database.EnsureDeleted();
			//Database.EnsureCreated();
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CitiesDb;Trusted_Connection=True;");
		}
	}

	

	public class EFWorker
	{
		//создание экземпляров сущностей, запускать только раз при старте проекта, потом закомментировать
		public static void DBPushData()
		{		
			using (ApplicationContext context = new ApplicationContext())
			{
				City c1 = new City { CityName = "Moscow", Temperature = 991, GenerationTime = 0};
				City c2 = new City { CityName = "Saint-Petersburg", Temperature = 992, GenerationTime = 0 };
				City c3 = new City { CityName = "Omsk", Temperature = 993, GenerationTime = 0 };
				City c4 = new City { CityName = "Chelyabinsk", Temperature = 994, GenerationTime = 0 };
				City c5 = new City { CityName = "Taganrog", Temperature = 995, GenerationTime = 0 };
				context.Cities.AddRange(c1, c2, c3, c4, c5);
				context.SaveChanges();
			}

		}


		public static bool CityTempChecker(String cityName, float temperature)
		{	
			//чтение города
			using (ApplicationContext context = new ApplicationContext())
			{
				Console.Clear();
				//если город нашёлся
				if (context.Cities.Any(a => a.CityName == cityName))
				{
					Log.Information("Город НАЙДЕН.");
					City city = context.Cities.First(a => a.CityName == cityName);
					//сверяем температуру в базе с новой темературой
					Console.WriteLine($"Темп в базе: {city.Temperature}");
					Console.WriteLine($"Темп к сравнению: {temperature}");
					if (city.Temperature==temperature)
					{
						Console.WriteLine("Температура не изменилась.");
						return false;
					}
					else
					{
						Console.WriteLine("Температура НЕ СОВПАДАЕТ! НАДО ПИСАТЬ В БАЗУ!");
						return true;
					}
				}
				else
				{
					Log.Information("Город не найден в базе.");
					return false;
				}
			}
		}


	}
}
