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

		/// <summary>
		/// Gets or sets the fallback subset that will be used in AutoDetectLanguage if no language could be detected.
		/// </summary>
		/// <value>The fallback subset.</value>
		public string FallbackSubset {
			get;
			set;
		}
		
		/// <summary>
		/// The selected subset that will be used to retrieve texts.
		/// </summary>
		protected static string selectedSubset = "";

		/// <summary>
		/// Gets the currently selected subset.
		/// </summary>
		/// <returns>The selected subset.</returns>
		public static string GetSelectedSubset()
		{
			return selectedSubset;
		}

		/// <summary>
		/// Detects the language that the user has set in the browser and sets the selectedSubset appropriately. If no language can be detected, FallbackSubset will be used. Can be overridden in derived classes to implement a custom language detection mechanism.
		/// </summary>
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

