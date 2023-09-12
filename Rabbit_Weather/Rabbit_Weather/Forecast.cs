using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Weather
{
		public class Forecast
		{
			public float latitude { get; set; }
			public float longitude { get; set; }
			public float generationtime_ms { get; set; }
			public int utc_offset_seconds { get; set; }
			public string timezone { get; set; }
			public string timezone_abbreviation { get; set; }
			public float elevation { get; set; }
			public Current_Weather current_weather { get; set; }
		}

		public class Current_Weather
		{
			public float temperature { get; set; }
			public float windspeed { get; set; }
			public int winddirection { get; set; }
			public int weathercode { get; set; }
			public int is_day { get; set; }
			public DateTimeOffset time { get; set; }
		}

}
