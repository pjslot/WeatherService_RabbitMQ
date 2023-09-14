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

		//проверяем изменилась ли температура относительно базы
		public static bool CityTempChecker(String cityName, float temperature)
		{	
			//чтение города
			using (ApplicationContext context = new ApplicationContext())
			{				
				//если город нашёлся
				if (context.Cities.Any(a => a.CityName == cityName))
				{
					City city = context.Cities.First(a => a.CityName == cityName);
					//сверяем температуру в базе с новой темературой
					Log.Information($"Температура в базе: {city.Temperature}");
					Log.Information($"Температура на сайте: {temperature}");
					if (city.Temperature==temperature)
					{
						Log.Information("Температура не изменилась. Ваимодействия с БД не требуется.");
						return false;
					}
					else
					{
						Log.Warning("Текущая температура не совпадает с температурой в базе данных. Запуск актуализации.");
						return true;
					}
				}
				else
				{
					Log.Error("Город не найден в базе.");
					return false;
				}
			}
		}


		//меняем температуру в базе на новую
		public static bool EditCityTemp(String cityName, float newTemperature)
		{
			using (ApplicationContext context = new ApplicationContext())
			{				
				//если город есть в базе
				if (context.Cities.Any(a => a.CityName == cityName))
				{
					City cityToChange = context.Cities.FirstOrDefault(a => a.CityName.Contains(cityName));
					cityToChange.Temperature = newTemperature;
					context.Cities.Update(cityToChange);		
					context.SaveChanges();
					Log.Information("Правка в базу внесена.");
					return true;
				}
				else
				{
					Log.Error("Город не найден в базе.");
					return false;
				}
			}
		}





	}
}
