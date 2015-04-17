using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2015
 * 
 * This is free software.
 * License: MIT
*/

namespace mkcs.libtisiweb {
	/*
	 * Base class for controller. Provides methods for handling fragment stores.
	*/
	public abstract class TisiController : Controller {
		public TisiController () {
			DefaultSubset = "";
		}
		
		protected Dictionary<string, IFragment> fragmentRepository = new Dictionary<string, IFragment>();
		protected string subset, pageTitle, pageShortTitle, pageLongTitle;

		private string _FragmentRepositoryXmlFile;
		/// <summary>
		/// Gets or sets the fragment repository XML file. If this property is set, the repository will be read immediately.
		/// </summary>
		/// <value>The fragment repository XML file.</value>
		public string FragmentRepositoryXmlFile {
			get {
				return _FragmentRepositoryXmlFile;
			}
			set {
				if (!string.IsNullOrEmpty(value) && System.IO.File.Exists(value)) {
					var doc = new XmlDocument();
					doc.Load(value);
					ReadXmlRepository(doc);
					_FragmentRepositoryXmlFile = value;
				}
				else {
					throw new ArgumentException("FragmentRepositoryXmlFile must not be null and the path set must be accessible.");
				}
			}
		}

		/// <summary>
		/// Gets or sets the default subset that is used in case the desired subset is not available for a given fragment.
		/// </summary>
		/// <value>The default subset.</value>
		public string DefaultSubset { get; set; }
		
		public string GetURIParameter(string parameter) {
			return this.ControllerContext.RouteData.GetRequiredString(parameter);
		}

		public string GetSubsetId() {
			return GetURIParameter("subset");
		}

		public void ReadXmlRepository(XmlDocument doc) {
			ProcessRepositoryNodes(doc.SelectNodes("/pages/page"));
		}

		public void ProcessRepositoryNodes(XmlNodeList nodes) {
			if (nodes == null || nodes.Count == 0)
				return;
			Trace.WriteLine("ProcessRepositoryNodes(" + nodes.Count.ToString() + ")");
			string pageName = "", fragmentName = "", subsetId = "";
			foreach (XmlNode node in nodes) {
				pageName = node.Attributes["name"].Value;
				foreach (XmlNode nodeFragment in node.SelectNodes("fragment")) {
					fragmentName = nodeFragment.Attributes["name"].Value;
					XmlNodeList subsetNodes = nodeFragment.SelectNodes("subset");
					// Does this fragment have subsets?
					if (subsetNodes.Count > 0) {
						// Yes, so let's add every subset and its text
						foreach (XmlNode nodeSubset in subsetNodes) {
							subsetId = nodeSubset.Attributes["lang"].Value;
							SetFragmentValue(pageName + "." + fragmentName, subsetId, nodeSubset.InnerText);
						}
					}
					else {
						// No, so add the fragment's node text
						SetFragmentValue(pageName + "." + fragmentName, "", nodeFragment.InnerText);
					}
				}
				ProcessRepositoryNodes(node.SelectNodes("page"));
			}
		}

		/// <summary>
		/// Adds the fragment specified to the repository, if it doesn't exist yet, otherwise updates the fragment in the repository.
		/// </summary>
		/// <returns><c>true</c>, if fragment was added, <c>false</c> if fragment was updated.</returns>
		/// <param name="fragment">The fragment that should be added or updated.</param>
		protected bool AddFragment(IFragment fragment) {
			if (fragmentRepository.ContainsKey(fragment.Name)) {
				fragmentRepository[fragment.Name] = fragment;
				Trace.WriteLine("AddFragment: Updated " + fragment.Name);
				return false;
			}
			else {
				fragmentRepository.Add(fragment.Name, fragment);
				Trace.WriteLine("AddFragment: Inserted " + fragment.Name);
				return true;
			}
		}

		protected override void Initialize (System.Web.Routing.RequestContext requestContext) {
			base.Initialize (requestContext);
			subset = GetSubsetId();
			var action = GetURIParameter("action");
			pageTitle = GetFragmentValue(action + ".title", subset);
			pageShortTitle = GetFragmentValue(action + ".shorttitle", subset);
			pageLongTitle = GetFragmentValue(action + ".longtitle", subset);
		}

		public void SetFragmentValue(string fragmentName, string fragmentSubset, string fragmentValue) {
			IFragment fragment;
			if (fragmentRepository.ContainsKey(fragmentName)) {
				fragment = fragmentRepository[fragmentName];
			}
			else {
				fragment = new Fragment();
				fragmentRepository.Add(fragmentName, fragment);
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
			if (fragmentRepository.ContainsKey(fragmentName)) {
				IFragment fragment = fragmentRepository[fragmentName];
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
					return "#SUBSET_NOT_FOUND("+ fragmentName + "," + fragmentSubset + ")";
				}
			}
			else {
				// Requested fragment is not available, so return a magic string that signalises a problem
				return "#FRAGMENT_NOT_FOUND(" + fragmentName + ")";
			}
		}
	}
}

