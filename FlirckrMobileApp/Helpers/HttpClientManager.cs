using System;
using System.Net.Http;
using Windows.Devices.Geolocation;
using FlirckrMobileApp.Model;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlirckrMobileApp.Helpers
{
	public class HttpClientManager
	{
		#region Fields

		private static HttpClient _client;

		#endregion

		#region Properties

		public static HttpClient Client
		{
			get { return _client ?? (_client = new HttpClient()); }
		}

		#endregion

		#region public methods

		public static async Task<FlickrMain> GetPhotosByGeolocation(double latitude, double longitude, int radius)
		{
			var response = await Client.GetStringAsync(GetServiceParametrizedUrl(latitude, longitude, radius));
			try
			{
				return TryParseResponse(response);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static Uri GetPhotoUrl(Photo photo)
		{
			return new Uri(string.Format(PreparedString.GetPhotoFromServerUrl, photo.Farm, photo.Server, photo.Id, photo.Secret));
		}

		#endregion

		#region Private methods

		private static FlickrMain TryParseResponse(string response)
		{
			return JsonConvert.DeserializeObject<FlickrMain>(RemoveRedundandPrefixes(response));
		}

		private static string RemoveRedundandPrefixes(string response)
		{
			response = response.Replace(PreparedString.ResponsePrefix, string.Empty);
			response = response.Remove(response.Length - 1);
			return response;
		}

		private static string GetServiceParametrizedUrl(double latitude, double longitude, int radius)
		{
			return string.Format(PreparedString.GetPhotosInformationUrl, PreparedString.ApiKey, latitude,
				longitude, radius);
		}

		#endregion
	}
}
