using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using FlirckrMobileApp.Common;
using FlirckrMobileApp.Model;
using Newtonsoft.Json;

namespace FlirckrMobileApp.ViewModel
{
	public class MainPageViewModel : BaseNotify
	{
		private readonly LocalizationManager _localizationManager;
		private Geopoint _centerPoint;
		private string _status;
		private string _point;

		public ObservableCollection<Photo> Photos { get; private set; }

		public MainPageViewModel()
		{
			Status = "Start";
			_localizationManager = new LocalizationManager();
			Photos = new ObservableCollection<Photo>();
			SetGeoPoint();
		}

		private async void SetGeoPoint()
		{
			Center = await _localizationManager.GetActualPoint();
			HttpClient client = new HttpClient();
			string url = string.Format(PreparedString.GetPhotosInformationUrl, PreparedString.ApiKey, Center.Position.Latitude,
				Center.Position.Longitude, 10);

			var response = await client.GetStringAsync(url);
			response = response.Replace(PreparedString.ResponsePrefix, string.Empty);
			response = response.Remove(response.Length - 1);

			FlickrMain flickrMain = JsonConvert.DeserializeObject<FlickrMain>(response);

			if (flickrMain.Stat == PreparedString.ValidStat)
			{
				Status = "Connection with server successful";
				foreach (var photo in flickrMain.Photos.Photo)
				{
					string photoUrl = string.Format(PreparedString.GetPhotoFromServerUrl, photo.Farm, photo.Server, photo.Id,
						photo.Secret);
					photo.PhotoUrl = new Uri(photoUrl);

					Photos.Add(photo);
					Status = "Photos in conllection == " + Photos.Count;
				}
			}
			else
			{
				Status = "Cant get data from serwer";
			}
		}

		public Geopoint Center
		{
			get
			{

				if (_centerPoint == null)
					Status = "Setting geo location";
				else
					Status = "Localization found";

				return _centerPoint;
			}

			set
			{
				_centerPoint = value;
				Point = value.Position.Latitude + " - " + value.Position.Longitude;

				NotifyPropertyChanged("Center");
			}
		}

		public string Status
		{
			get { return _status; }
			set
			{
				_status = value;
				NotifyPropertyChanged("Status");
			}
		}

		public string Point
		{
			get { return _point; }
			set
			{
				_point = value;
				NotifyPropertyChanged("Point");
			}
		}
	}
}
