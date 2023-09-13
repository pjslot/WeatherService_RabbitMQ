using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Weather
{
	
	internal class InfoGrabber
	{
		//ручная печать прогноза для отладки
		public static async Task PrintInfo (Location loc)
		{
			Log.Information("PrintInfo Method Run:");
			var wb = new WebClient();		
			try
			{			
				string latStr = loc.latitude.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
				string lonStr = loc.longitude.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
				var answer = await wb.DownloadStringTaskAsync($"https://api.open-meteo.com/v1/forecast?latitude={latStr}&longitude={lonStr}&current_weather={loc.current_weather}");
				Console.WriteLine(answer.ToString());
				Log.Information("PrintInfo Method Succesfully Finished.");
			}
			catch(Exception e)
			{
				Console.WriteLine($"ERROR!: {e.Message}");
			}
			finally
			{
				wb.Dispose();
			}		
		}

		//асинхронный возврат строки прогноза
		 public static async Task<string> ReturnInfo (Location loc)
		{
			var wb = new WebClient();
			try
			{
				string latStr = loc.latitude.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
				string lonStr = loc.longitude.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
				var answer = await wb.DownloadStringTaskAsync($"https://api.open-meteo.com/v1/forecast?latitude={latStr}&longitude={lonStr}&current_weather=true");
				//Log.Information("ReturnInfo Method Succesfully Finished.");
				return answer;
			}
			catch (Exception e)
			{
				Console.WriteLine($"ERROR!: {e.Message}");
			}
			finally
			{				
				wb.Dispose();
			}
			return null;

		}
	}
}
