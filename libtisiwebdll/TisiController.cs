using System;
using System.Collections.Generic;
using System.Web.Mvc;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2015
 * 
 * This is free software.
 * License: MIT
*/

namespace mkcs.libtisiweb
{
	/*
	 * Base class for controller. Provides methods for handling fragment stores.
	*/
	public abstract class TisiController : Controller
	{
		public TisiController ()
		{
		}

		//protected Dictionary<string, Dictionary<string, string>> fragmentRepository = new Dictionary<string, Dictionary<string, string>>();
		protected List<Fragment> fragmentRepository = new List<Fragment> ();

		protected string GetFragmentValue(string fragmentName, string subset) {
			if (fragmentRepository.ContainsKey(fragmentName)) {
				Dictionary<string, string> fragment = fragmentRepository[fragmentName];
				if (!fragment.ContainsKey(subset) && subset.IndexOf("-") > 0) {
					subset = subset.Substring(0, subset.IndexOf("-"));
				}
				if (!fragment.ContainsKey(subset)) {
					subset = "en";
				}
				if (fragment.ContainsKey(subset)) {
					return fragment[subset];
				}
				else {
					return "#SUBSET_NOT_FOUND("+ fragmentName + "," + subset + ")";
				}
			}
			else {
				return "#FRAGMENT_NOT_FOUND(" + fragmentName + ")";
			}
		}
	}
}

