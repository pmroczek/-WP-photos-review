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
        #region Fields

        private readonly LocalizationManager _localizationManager;
        private Geopoint _centerPoint;
        private int _pivotIndex;
        private int _radius;
        private string _status;

        #endregion

        #region Properties

        public ObservableCollection<Photo> Photos { get; private set; }

        public ObservableCollection<Photo> FavoritePhotos { get; private set; }

        public int PivotIndex
        {
            get { return _pivotIndex; }
            set
            {
                _pivotIndex = value;
                NotifyPropertyChanged("PivotIndex");
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

        #endregion

        #region Contructor

        public MainPageViewModel()
        {
            Status = "Search location...";
            _localizationManager = new LocalizationManager();
            Photos = new ObservableCollection<Photo>();
            FavoritePhotos = new ObservableCollection<Photo>();
            InitializeMapAndPhoto();
        }

        #endregion

        #region Public methods

        public async void InitializeMapAndPhoto()
        {
            await GetGeoLocation();
            Status = "Found location.";
            GetPhotos();
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

        public void DeleteFavorite(Uri uri)
        {
            var photo = FavoritePhotos.FirstOrDefault(c => c.PhotoUrl == uri);

            if (photo != null)
            {
                FavoritePhotos.Remove(photo);
                RemovePhotoFormIsolatedStorage(photo);
            }
        }

        #endregion

        #region Privete methods

        private async Task GetGeoLocation()
        {
            Center = await _localizationManager.GetActualPoint();
        }

        public async void GetPhotos()
        {
            ClearCollection(Photos);

            var flickrMain =
                await HttpClientManager.GetPhotosByGeolocation(Center.Position.Latitude, Center.Position.Longitude, Radius);

            if (flickrMain.Stat == PreparedString.ValidStat)
            {
                foreach (var photo in flickrMain.Photos.Photo)
                {
                    photo.PhotoUrl = HttpClientManager.GetPhotoUrl(photo);
                    Photos.Add(photo);
                }
                PivotIndex = 1;
                Status = string.Format("Found {0} photos", Photos.Count);
            }
            else
            {
                Status = "Error while getting photos. Try again later.";
                PivotIndex = 2;
            }
        }


        private static void SavePhotoInIsolatedStorage(Photo photo)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            string key = photo.Id;
            settings.Values[key] = photo.PhotoUrl.OriginalString;
        }

        public void ReadPhotosFromIsolatedStorage()
        {
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

        private static void RemovePhotoFormIsolatedStorage(Photo photo)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(photo.Id))
                settings.Values.Remove(photo.Id);
        }

        private void ClearCollection(ObservableCollection<Photo> list)
        {
            {
                while (list.Count > 0)
                {
                    list.RemoveAt(Photos.Count - 1);
                }
            }
        }

        #endregion
    }
}
