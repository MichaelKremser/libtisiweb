using System;
using System.Collections.Generic;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2015
 * 
 * This is free software.
 * License: MIT
*/

namespace mkcs.libtisiweb {

	public interface IFragmentRepository : IDictionary<string, IFragment> {
		void SetFragmentValue(string fragmentName, string fragmentSubset, string fragmentValue);
		string GetFragmentValue(string fragmentName, string fragmentSubset);
		string DefaultSubset { get; set; }
		// Normally inherited from base class of implementing class
		//int Count { get; }
	}

	public class FragmentRepository : Dictionary<string, IFragment>, IFragmentRepository {

		public FragmentRepository () {
		}

		public string DefaultSubset { get; set; } // Normally, this property is set in TisiController (see property with same name there)

//		public void AddFragment(string fragmentName, IFragment fragmentInstance) {
//			this.Add(fragmentName, fragmentInstance);
//		}

		public void SetFragmentValue(string fragmentName, string fragmentSubset, string fragmentValue) {
			IFragment fragment;
			if (this.ContainsKey(fragmentName)) {
				fragment = this[fragmentName];
			}
			else {
				fragment = new Fragment();
				this.Add(fragmentName, fragment);
			}
			if (fragment.ContainsSubset(fragmentSubset)) {
				fragment.Subsets[fragmentSubset] = fragmentValue;
			}
			else {
				fragment.AddSubset(fragmentSubset, fragmentValue);
			}
		}

		public string GetFragmentValue(string fragmentName, string fragmentSubset) {
			// Is the requested fragment available?
			if (this.ContainsKey(fragmentName)) {
				IFragment fragment = this[fragmentName];
				// Check if the fragment contains any subset rather then the "undefined" subset at all
				if (fragment.ContainsSubset("")) {
					// No, it doesn't, so reset the requested subset to an empty string
					fragmentSubset = "";
				}
				else {
					// Yes, it does, so try to find an appropriate subset
					// If the requested subset is for instance "en-us", but that's not available, try with "en" only
					if (!fragment.ContainsSubset(fragmentSubset) && fragmentSubset.IndexOf("-") > 0) {
						fragmentSubset = fragmentSubset.Substring(0, fragmentSubset.IndexOf("-"));
					}
					// If the requested subset is not available, fall back to default subset
					if (!fragment.ContainsSubset(fragmentSubset)) {
						fragmentSubset = DefaultSubset;
					}
				}
				// Return the requested subset if availble
				if (fragment.ContainsSubset(fragmentSubset)) {
					return fragment.Subsets[fragmentSubset];
				}
				else {
					// Otherwise return a magic string that signalises a problem
					return "#SUBSET_NOT_FOUND('"+ fragmentName + "','" + fragmentSubset + "')";
				}
			}
			else {
				// Requested fragment is not available, so return a magic string that signalises a problem
				return "#FRAGMENT_NOT_FOUND('" + fragmentName + "')";
			}
		}
	}
}
