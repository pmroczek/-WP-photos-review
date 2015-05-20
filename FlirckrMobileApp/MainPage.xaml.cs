using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System.ComponentModel;
using Windows.UI.Xaml.Controls.Maps;
using FlirckrMobileApp.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace FlirckrMobileApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            DataContext = App.MainPageViewModel;
        }

        private void FavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.CommandParameter is Uri)
                App.MainPageViewModel.AddFavorite(button.CommandParameter as Uri);
        }

        private void RefreshLocation_OnClick(object sender, RoutedEventArgs e)
        {
            App.MainPageViewModel.InitializeMapAndPhoto();
        }

        private void DeleteFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.CommandParameter is Uri)
                App.MainPageViewModel.DeleteFavorite(button.CommandParameter as Uri);
        }

        private void BtnPhotosPlace_OnClick(object sender, RoutedEventArgs e)
        {
            App.MainPageViewModel.GetPhotos();
        }
    }
}
