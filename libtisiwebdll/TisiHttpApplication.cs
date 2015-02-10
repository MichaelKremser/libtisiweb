using System;
using System.Web;
using System.Web.Mvc;

namespace mkcs.libtisiweb
{
	public abstract class TisiHttpApplication : HttpApplication
	{
		public TisiHttpApplication ()
		{
			FallbackSubset = "en";
		}

		private static string selectedSubset = "";

		public string FallbackSubset {
			get;
			set;
		}

		public static string GetSelectedSubset()
		{
			return selectedSubset;
		}

		public void AutoDetectLanguage()
		{
			if (HttpContext.Current != null && HttpContext.Current.Request != null) {
				HttpRequest Request = HttpContext.Current.Request;
				if (Request.UserLanguages != null && Request.UserLanguages.Length > 0) {
					selectedSubset = Request.UserLanguages[0];
				}
			}
			if (string.IsNullOrEmpty (selectedSubset)) {
				selectedSubset = FallbackSubset;
			}
		}
	}
}

