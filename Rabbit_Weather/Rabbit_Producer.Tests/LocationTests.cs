using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rabbit_Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Weather.Tests
{
	[TestClass()]
	public class LocationTests
	{
		[TestMethod()]
		public void LocationTest()
		{
			//arrange
			string cityName = "Test City";
			float latitude = 10.1F;
			float longitude = 20.2F;
			bool currentWeather = true;

			string cityNameExpected = "Test City";
			float latitudeExpected = 10.1F;
			float longitudeExpected = 20.2F;
			bool currentWeatherExpected = true;

			//act
			Location loc = new Location(cityName, latitude, longitude, currentWeather);
			
			//assert
			Assert.AreEqual(cityNameExpected, loc.cityName);
			Assert.AreEqual(latitudeExpected, loc.latitude);
			Assert.AreEqual(longitudeExpected, loc.longitude);
			Assert.AreEqual(currentWeatherExpected, loc.current_weather);
		}
	}
}