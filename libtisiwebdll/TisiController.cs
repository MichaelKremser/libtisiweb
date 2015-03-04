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
		protected Dictionary<string, Fragment> fragmentRepository = new Dictionary<string, Fragment>();

		protected string GetFragmentValue(string fragmentName, string subset) {
			if (fragmentRepository.ContainsKey(fragmentName)) {
				IFragment fragment = fragmentRepository[fragmentName];
				if (!fragment.ContainsSubset(subset) && subset.IndexOf("-") > 0) {
					subset = subset.Substring(0, subset.IndexOf("-"));
				}
				if (!fragment.ContainsSubset(subset)) {
					subset = "en";
				}
				if (fragment.ContainsSubset(subset)) {
					return fragment.Subsets[subset];
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

