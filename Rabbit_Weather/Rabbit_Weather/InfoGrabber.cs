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
		public static async void PrintInfo (Location loc)
		{
			Console.WriteLine("Start");
			var wb = new WebClient();		
			try
			{			
				string latStr = loc.latitude.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
				string lonStr = loc.longitude.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

				Console.WriteLine("RESULT:");
				var answer =  wb.DownloadString($"https://api.open-meteo.com/v1/forecast?latitude={latStr}&longitude={lonStr}&current_weather=true");				
				Console.WriteLine(answer.ToString());			
			}
			catch(Exception e)
			{
				Console.WriteLine($"ERROR!: {e.Message}");
			}
			finally
			{
				wb.Dispose();
				Console.WriteLine("FINISHED. DISPOSED.");
			}		
		}

		public dynamic ReturnInfo (Location loc)
		{
			var wb = new WebClient();
			try
			{
				string latStr = loc.latitude.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
				string lonStr = loc.longitude.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

				var answer = wb.DownloadString($"https://api.open-meteo.com/v1/forecast?latitude={latStr}&longitude={lonStr}&current_weather=true");
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
