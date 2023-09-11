using System.Net.Http;
using System.Net;
using System;


namespace Rabbit_Weather
{
	

	internal class Program
	{
		static void Main(string[] args)
		{
			

			Location loc = new Location(52.52F, 13.41F, true) ;

			Console.WriteLine("Main Start");

			InfoGrabber.PrintInfo(loc);

			Console.WriteLine("Main Finish");
		}

		//static async Task<HttpResponseMessage> GetRequest (string address, Dictionary<string, string> Params)
		//{
		//	HttpClient client = new HttpClient();
			

		//	try
		//	{
		//		Uri uri = new Uri(address);
		//		var content = new FormUrlEncodedContent(Params);

		//		return await client.PostAsync(address, content);
		//	}
		//	catch (Exception x) 
		//	{
		//		Console.WriteLine($"Error: {x.ToString}");
		//	}
			

		//	return null;
			
		//}
	}
}