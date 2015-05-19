using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlirckrMobileApp
{
	public class PreparedString
	{
		public const string ApiKey = "9e887b1d59c64092d64803918b9db6e4";

		public const string GetPhotosInformationUrl = "https://api.flickr.com/services/rest/" +
						 "?method=flickr.photos.search" +
						 "&api_key={0}" +
						 "&lat={1}" +
						 "&lon={2}" +
						 "&radius={3}" +
						 "&format=json";

		public const string GetPhotoFromServerUrl = "http://farm{0}.staticflickr.com/{1}/{2}_{3}_n.jpg";

		public const string ResponsePrefix = "jsonFlickrApi(";

		public const string ValidStat = "ok";

		public const string InvalidStat = "fail";
	}
}
