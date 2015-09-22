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
		
		//protected Dictionary<string, IFragment> fragmentRepository = new Dictionary<string, IFragment>();
		protected IFragmentRepository fragmentRepository = new FragmentRepository();
		protected string subset, action, pageTitle, pageShortTitle, pageLongTitle;

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
					//ReadXmlRepository(doc);
					var xmlFileReader = new XmlFragmentRepositoryReader();
					xmlFileReader.ReadFragmentRepository(doc, fragmentRepository);
					_FragmentRepositoryXmlFile = value;
				}
				else {
					throw new ArgumentException("FragmentRepositoryXmlFile must not be null and the path set must be accessible.");
				}
			}
		}

		public string PageTitle { get { return pageTitle; } }
		public string PageShortTitle { get { return pageShortTitle; } }
		public string PageLongTitle { get { return pageLongTitle; } }

		public IFragmentRepository FragmentRepository {
			get {
				return this.fragmentRepository;
			}
			set {
				this.fragmentRepository = value;
			}
		}

		/// <summary>
		/// Gets or sets the default subset that is used in case the desired subset is not available for a fragment given.
		/// </summary>
		/// <value>The default subset.</value>
		public string DefaultSubset {
			get {
				return fragmentRepository.DefaultSubset;
			}
			set {
				fragmentRepository.DefaultSubset = value;
			}
		}
		
		public string GetURIParameter(string parameter) {
			return this.ControllerContext.RouteData.GetRequiredString(parameter);
		}

		public string GetSubsetId() {
			return GetURIParameter("subset");
		}

		public string GetAction() {
			return GetURIParameter("action");
		}

		protected override void Initialize (System.Web.Routing.RequestContext requestContext) {
			base.Initialize (requestContext);
			subset = GetSubsetId();
			action = GetAction();
			pageTitle = fragmentRepository.GetFragmentValue(action + ".title", subset);
			pageShortTitle = fragmentRepository.GetFragmentValue(action + ".shorttitle", subset);
			pageLongTitle = fragmentRepository.GetFragmentValue(action + ".longtitle", subset);
			ViewData["subset"] = subset;
			ViewData["action"] = action;
			ViewData["FragmentRepository"] = fragmentRepository;
		}

	}
}

