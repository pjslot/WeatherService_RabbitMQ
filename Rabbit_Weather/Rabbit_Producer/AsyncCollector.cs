using Rabbit_Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Producer
{
	public class AsyncCollector
	{
		//статичный лист с прогнозами
		static List<string> result = new List<string>();


		public static async Task Returner(Dictionary<int, Location> locations)
		{

			Task<string> callbackResult = null;	
			result.Clear();	

			foreach (KeyValuePair<int, Location> entry in locations)
			{
				Console.WriteLine(entry.Key + entry.Value.cityName);
				callbackResult = InfoGrabber.ReturnInfo(entry.Value);
				string res = await callbackResult;
				result.Add(res);
			}
		}


		//выдача листа с прогнозами
		public static List<string> Returned()

		{
			return result;
		}


	}
}
