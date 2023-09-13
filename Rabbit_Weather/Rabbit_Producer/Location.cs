using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Weather
{
	public class Location
	{
		public string cityName { get; set; }
		public float latitude { get; set; }
		public float longitude { get; set; }
		public bool current_weather { get; set; }

		public Location(string cityName, float latitude, float longitude, bool current_weather)
		{
			this.cityName = cityName;
			this.latitude = latitude;
			this.longitude = longitude;
			this.current_weather = current_weather;
		}
	}
}
