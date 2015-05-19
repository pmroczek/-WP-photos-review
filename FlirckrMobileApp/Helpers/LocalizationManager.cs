using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace FlirckrMobileApp.Helpers
{
	public class LocalizationManager
	{
		private Geolocator _geolocator;

		public static Geopoint DefaultLocation
		{
			get
			{
				return new Geopoint(new BasicGeoposition()
				{
					Latitude = 54.338989,
					Longitude = 18.653728
				});
			}
		}

		public static double DefaultZoom
		{
			get
			{
				return 11;
			}
		}

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
