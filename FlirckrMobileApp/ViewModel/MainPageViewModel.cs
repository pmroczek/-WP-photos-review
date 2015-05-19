using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Media.Devices;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FlirckrMobileApp.Common;
using FlirckrMobileApp.Helpers;
using FlirckrMobileApp.Model;
using Newtonsoft.Json;

namespace FlirckrMobileApp.ViewModel
{
	public class MainPageViewModel : BaseNotify
	{
		private readonly LocalizationManager _localizationManager;
		private Geopoint _centerPoint;
		//private double _zoom;
		//private string _point;
		private int _radius;
		private string _status;

		public ObservableCollection<Photo> Photos { get; private set; }

		public ObservableCollection<Photo> FavoritePhotos { get; private set; }

		//public DelegateCommand<ItemClickEventArgs> ItemClickedCommand { get; set; }
		public MainPageViewModel()
		{
			//Zoom = LocalizationManager.DefaultZoom;
			Status = "Start";
			_localizationManager = new LocalizationManager();
			Photos = new ObservableCollection<Photo>();
			FavoritePhotos = new ObservableCollection<Photo>();
			FavoritePhotos.CollectionChanged += FavoritePhotosOnCollectionChanged;
			InitializeMapAndPhoto();
			//ItemClickedCommand = new DelegateCommand<ItemClickEventArgs>(OnItemClicked);
		}

		//private void OnItemClicked(ItemClickEventArgs obj)
		//{
		//	Photo item = obj.ClickedItem as Photo;
		//	FavoritePhotos.Add(item);
		//}

		private void FavoritePhotosOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
		}

		public async void InitializeMapAndPhoto()
		{
			//Center = LocalizationManager.DefaultLocation;
			Radius = 1;
			await GetGeoLocation();
			GetPhotos();
		}

		private async Task GetGeoLocation()
		{
			Center = await _localizationManager.GetActualPoint();
			//Zoom = 14;
			//GetPhotos();
		}

		private async void GetPhotos()
		{
			ClearCollection();

			var flickrMain =
				await HttpClientManager.GetPhotosByGeolocation(Center.Position.Latitude, Center.Position.Longitude, Radius);

			if (flickrMain.Stat == PreparedString.ValidStat)
			{
				Status = "Connection with server successful";

				foreach (var photo in flickrMain.Photos.Photo)
				{
					photo.PhotoUrl = HttpClientManager.GetPhotoUrl(photo);
					Photos.Add(photo);
				}

				Status = "Photos in conllection == " + Photos.Count;
			}
			else
				Status = "Server connection error";
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
				//Point = value.Position.Latitude + " - " + value.Position.Longitude;
				NotifyPropertyChanged("Center");
			}
		}

		//public double Zoom
		//{
		//	get { return _zoom; }
		//	set
		//	{
		//		_zoom = value;
		//		NotifyPropertyChanged("Zoom");
		//	}
		//}

		public string Status
		{
			get { return _status; }
			set
			{
				_status = value;
				NotifyPropertyChanged("Status");
			}
		}

		//public string Point
		//{
		//	get { return _point; }
		//	set
		//	{
		//		_point = value;
		//		NotifyPropertyChanged("Point");
		//	}
		//}

		public int Radius
		{
			get { return _radius; }
			set
			{
				_radius = value / 10;

				if (_centerPoint != null)
					GetPhotos();

				NotifyPropertyChanged("Radius");
			}
		}

		public void AddFavorite(Uri photoUri)
		{
			var photo = Photos.FirstOrDefault(c => c.PhotoUrl == photoUri);
			if (photo != null)
			{
				FavoritePhotos.Add(photo);
				SavePhotoInIsolatedStorage(photo);
			}
		}

		private void SavePhotoInIsolatedStorage(Photo photo)
		{
			ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
			string key = photo.Id;
			settings.Values[key] = photo.PhotoUrl.OriginalString;
		}

		public void ReadPhotosFromIsolatedStorage()
		{
			ClearFavoriteCollection();
			ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
			if (!settings.Values.Any()) return;
			{
				foreach (var photo in settings.Values)
				{
					FavoritePhotos.Add(new Photo()
					{
						Id = photo.Key,
						PhotoUrl = new Uri(photo.Value.ToString())
					});
				}
			}
		}

		public void DeleteFavorite(Uri uri)
		{
			var photo = FavoritePhotos.FirstOrDefault(c => c.PhotoUrl == uri);

			if (photo != null)
				FavoritePhotos.Remove(photo);
		}

		private void ClearCollection()
		{
			{
				while (Photos.Count > 0)
				{
					Photos.RemoveAt(Photos.Count - 1);
				}
			}
		}

		private void ClearFavoriteCollection()
		{
			{
				while (FavoritePhotos.Count > 0)
				{
					FavoritePhotos.RemoveAt(Photos.Count - 1);
				}
			}
		}
	}
}
