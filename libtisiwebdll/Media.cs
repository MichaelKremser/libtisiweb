using System;
using System.Collections.Generic;

namespace libtisiwebdll {

	/// <summary>
	/// Represents information about media like a picture, a video, an audio file or the like.
	/// </summary>
	public class Media {
		public Media () {
		}

		public MediaType MediaType { get; set; }
		public string ID { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public long Filesize { get; set; }
		public int Rotation { get; set; }
		public long Width { get; set; }
		public long Height { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }
		public double Elevation { get; set; }
		public string Author { get; set; }
		public string Device { get; set; }
		public string CustomData { get; set; }
		public List<string> Tags { get; set; }
	}
}

