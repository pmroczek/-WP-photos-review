﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlirckrMobileApp.Model
{
	public class Photos
	{
		public int Page { get; set; }
		public int Pages { get; set; }
		public int Perpage { get; set; }
		public string Total { get; set; }
		public List<Photo> Photo { get; set; }
	}
}
