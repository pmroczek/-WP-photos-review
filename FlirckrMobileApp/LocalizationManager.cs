using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace FlirckrMobileApp
{
	public class LocalizationManager
	{
		private Geolocator _geolocator;

		public LocalizationManager()
		{
			_geolocator = new Geolocator();
			_geolocator.DesiredAccuracyInMeters = 100;
		}

		public async Task<Geopoint> GetActualPoint()
		{
			Geoposition position = await _geolocator.GetGeopositionAsync(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30));
			return position.Coordinate.Point;
		}
	}
}
