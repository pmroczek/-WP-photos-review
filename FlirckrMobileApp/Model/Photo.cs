using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Notification.Management;
using FlirckrMobileApp.Common;

namespace FlirckrMobileApp.Model
{
	public class Photo : BaseNotify
	{
		private Uri _photoUrl;

		public string Id { get; set; }
		public string Owner { get; set; }
		public string Secret { get; set; }
		public string Server { get; set; }
		public int Farm { get; set; }
		public string Title { get; set; }
		public int Ispublic { get; set; }
		public int Isfriend { get; set; }
		public int Isfamily { get; set; }

		public Uri PhotoUrl
		{
			get { return _photoUrl; }
			set
			{
				_photoUrl = value;
				NotifyPropertyChanged("PhotoUrl");
			}
		}
	}
}
