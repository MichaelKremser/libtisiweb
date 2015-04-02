using System;
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
			string pageName = "", fragmentName = "", subsetId = "";
			foreach (XmlNode node in nodes) {
				pageName = node.Attributes["name"].Value;
				foreach (XmlNode nodeFragment in node.SelectNodes("fragment")) {
					fragmentName = nodeFragment.Attributes["name"].Value;
					foreach (XmlNode nodeSubset in nodeFragment.SelectNodes("subset")) {
						subsetId = nodeSubset.Attributes["lang"].Value;
						SetFragmentValue(pageName + "." + fragmentName, subsetId, nodeSubset.InnerText);
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
				return false;
			}
			else {
				fragmentRepository.Add(fragment.Name, fragment);
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

		public void SetFragmentValue(string fragmentName, string subset, string fragmentValue) {
			IFragment fragment;
			if (fragmentRepository.ContainsKey(fragmentName)) {
				fragment = fragmentRepository[fragmentName];
			}
			else {
				fragment = new Fragment();
				fragmentRepository.Add(fragmentName, fragment);
			}
			if (fragment.ContainsSubset(subset)) {
				fragment.Subsets[subset] = fragmentValue;
			}
			else {
				fragment.AddSubset(subset, fragmentValue);
			}
		}

		public string GetFragmentValue(string fragmentName, string subset) {
			if (fragmentRepository.ContainsKey(fragmentName)) {
				IFragment fragment = fragmentRepository[fragmentName];
				if (!fragment.ContainsSubset(subset) && subset.IndexOf("-") > 0) {
					subset = subset.Substring(0, subset.IndexOf("-"));
				}
				if (!fragment.ContainsSubset(subset)) {
					subset = DefaultSubset;
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

