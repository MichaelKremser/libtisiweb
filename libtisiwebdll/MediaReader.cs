using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2015
 * 
 * This is free software.
 * License: MIT
*/

namespace mkcs.libtisiweb {

	public class MediaReader {

		public MediaReader() {
		}

		public List<Media> ReadMediaListFromPath(DirectoryInfo Media, DirectoryInfo MediaThumbs, DirectoryInfo MediaMidRes, MediaType MediaTypeToSearchFor) {
			if (Media == null)
				throw new ArgumentNullException("Media");
			if (MediaThumbs == null)
				throw new ArgumentNullException("MediaThumbs");
			if (!(MediaTypeToSearchFor != MediaType.Picture) || (MediaTypeToSearchFor != MediaType.Audio) || (MediaTypeToSearchFor != MediaType.Video))
				throw new ArgumentException("The requested media type cannot be searched for.");
			string[] searchPatterns = null;
			if (MediaTypeToSearchFor == MediaType.Picture) {
				searchPatterns = new string[] { "jpg", "png", "gif" };
			}
			if (MediaTypeToSearchFor == MediaType.Audio) {
				searchPatterns = new string[] { "ogg", "wav", "mp3", "flac" };
			}
			if (MediaTypeToSearchFor == MediaType.Video) {
				searchPatterns = new string[] { "avi", "mpeg", "mp4" };
			}
			var mediaList = new List<Media>();
			Media media;
			foreach (var searchPattern in searchPatterns) {
				foreach (FileInfo fileInfo in Media.GetFiles(searchPattern)) {
					media = new Media();
					media.Modified = fileInfo.LastWriteTime;
					media.ID = fileInfo.Name;
					media.Description = new Fragment();
					media.Description.Name = fileInfo.Name;
					media.Description.AddSubset("", "Picture " + fileInfo.Name);
					mediaList.Add(media);
				}
			}
			/*mediaList = (from media in mediaList
				orderby media.Modified
				select media).;*/
			return mediaList;
		}
	}
}

