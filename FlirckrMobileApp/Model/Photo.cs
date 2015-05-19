using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Notification.Management;
using FlirckrMobileApp.Common;

namespace FlirckrMobileApp.Model
{
	public class Photo : BaseNotify
	{
		private Uri _photoUrl;

		[DataMember]
		public string Id { get; set; }

		[DataMember]
		public string Owner { get; set; }

		[DataMember]
		public string Secret { get; set; }

		[DataMember]
		public string Server { get; set; }

		[DataMember]
		public int Farm { get; set; }

		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public int Ispublic { get; set; }

		[DataMember]
		public int Isfriend { get; set; }

		[DataMember]
		public int Isfamily { get; set; }

		[DataMember]
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
