﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Consumer
{
	//сущность города
	public class City
	{
		[Key]
		public int Id { get; set; }
		public string? CityName { get; set; }
		public float Temperature { get; set; }
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
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SecondCitiesDb;Trusted_Connection=True;");
		}
	}

	public class EFWorker
	{
		
		//правка существующей либо добавление новой сущности
		public static bool CityEditOrCreate(String cityName, float newTemperature)
		{
			//чтение города
			using (ApplicationContext context = new ApplicationContext())
			{
				//если город нашёлся
				if (context.Cities.Any(a => a.CityName == cityName))
				{
					//вносим изменения в данные по городу
					City cityToChange = context.Cities.FirstOrDefault(a => a.CityName.Contains(cityName));

					Log.Information($"Temp in DB: {cityToChange.Temperature}");
					Log.Information($"Rabbit incoming temp: {newTemperature}");

					cityToChange.Temperature = newTemperature;
					context.Cities.Update(cityToChange);
					context.SaveChanges();
					Log.Information("DB updated");
					return true;
				}
				//если не нашёлся - создаём
				else
				{
					Log.Information("New city creation started.");
					City cityNew = new City();
					cityNew.CityName = cityName;
					cityNew.Temperature = newTemperature;
					context.Cities.AddRange(cityNew);
					context.SaveChanges();
					Log.Information("DB updated");
					return true;
				}
			}
		}
	}
}
