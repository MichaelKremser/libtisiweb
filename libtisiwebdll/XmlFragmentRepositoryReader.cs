using System;
using System.Xml;
using System.Diagnostics;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2015
 * 
 * This is free software.
 * License: MIT
*/

namespace mkcs.libtisiweb {

	public class XmlFragmentRepositoryReader : IFragmentRepositoryReader<XmlDocument> {

		public XmlFragmentRepositoryReader () {
			PathToPagesNode = "/pages/";
			PageNodeName = "page";
			FragmentNodeName = "fragment";
			SubsetNodeName = "subset";
			SubsetNameAttribute = "lang";
		}

		public string PathToPagesNode { get; set; }
		public string PageNodeName { get; set; }
		public string FragmentNodeName { get; set; }
		public string SubsetNodeName { get; set; }
		public string SubsetNameAttribute { get; set; }

		public void ReadFragmentRepository(XmlDocument doc, IFragmentRepository fragmentRepository) {
			ProcessRepositoryNodes(doc.SelectNodes(PathToPagesNode + PageNodeName), fragmentRepository);
		}

		public void ProcessRepositoryNodes(XmlNodeList nodes, IFragmentRepository fragmentRepository) {
			if (fragmentRepository == null)
				throw new ArgumentNullException("fragmentRepository");
			if (nodes == null || nodes.Count == 0)
				return;
			Console.WriteLine("ProcessRepositoryNodes(" + nodes.Count.ToString() + ")");
			string pageName = "", fragmentName = "";
			foreach (XmlNode node in nodes) {
				ProcessPageNode(node, fragmentRepository);
			}
			Console.WriteLine("ProcessRepositoryNodes exit");
		}

		/// <summary>
		/// Processes a "page" node that has "fragment" child nodes.
		/// </summary>
		/// <param name="node">The XML node that should be processed. Must not be null.</param>
		/// <param name="fragmentRepository">The fragment repository where the information observed is written to. Must not be null.</param>
		/// <description>
		/// Iterates through all children of the node given that have a name like the contents of
		/// property "FragmentNodeName" (by default "fragment") and processes them.
		/// </description>
		public void ProcessPageNode(XmlNode node, IFragmentRepository fragmentRepository) {
			if (node == null)
				throw new ArgumentNullException("node");
			if (fragmentRepository == null)
				throw new ArgumentNullException("fragmentRepository");
			pageName = Xml.GetAttributeValueDefensive(node, "name");
			if (pageName.Length == 0) {
				Console.WriteLine ("\tSkipping unnamed page");
			}
			else {
				Console.WriteLine ("\tProcessing page node '" + pageName + "'");
				foreach (XmlNode nodeFragment in node.SelectNodes (FragmentNodeName)) {
					string fragmentName = Xml.GetAttributeValueDefensive (nodeFragment, "name");
					if (fragmentName.Length == 0) {
						Console.WriteLine ("\tSkipping unnamed fragment");
					}
					else {
						Console.WriteLine ("\tProcessing fragment node '" + fragmentName + "'");
						ProcessFragmentNode (pageName, fragmentName, nodeFragment, fragmentRepository);
					}
				}
				ProcessRepositoryNodes (node.SelectNodes (PageNodeName), fragmentRepository);
			}
		}

		/// <summary>
		/// Processes a "fragment" node that has either text or "subset" child nodes.
		/// </summary>
		/// <param name="pageName">The name of the page that is processed. This is used as a part of the fragment's name for the repository.</param>
		/// <param name="fragmentName">The name of the fragment.</param>
		/// <param name="nodeFragment">The XML node "fragment".</param>
		/// <param name="fragmentRepository">The fragment repository where the information observed is written to. Must not be null.</param>
		public void ProcessFragmentNode(string pageName, string fragmentName, XmlNode nodeFragment, IFragmentRepository fragmentRepository) {
			if (string.IsNullOrEmpty(fragmentName))
				throw new ArgumentNullException("fragmentName");
			if (nodeFragment == null)
				throw new ArgumentNullException("nodeFragment");
			if (fragmentRepository == null)
				throw new ArgumentNullException("fragmentRepository");
			XmlNodeList subsetNodes = nodeFragment.SelectNodes(SubsetNodeName);
			string subsetId = "";
			// Does this fragment have subsets?
			if (subsetNodes.Count > 0) {
				int subsetNodePtr = 0;
				// Yes, so let's add every subset and its text
				foreach (XmlNode nodeSubset in subsetNodes) {
					Console.WriteLine ("\tProcessing subset node #'" + (++subsetNodePtr).ToString () + "'");
					subsetId = Xml.GetAttributeValueDefensive (nodeSubset, SubsetNameAttribute);
					// nodeSubset.Attributes[SubsetNameAttribute].Value;
					if (subsetId.Length == 0) {
						Console.WriteLine ("\tNot adding subset without name");
					}
					else {
						fragmentRepository.SetFragmentValue((pageName == null ? "" : pageName) + "." + fragmentName, subsetId, nodeSubset.InnerText);
					}
				}
			}
			else {
				// No, so add the fragment's node text
				fragmentRepository.SetFragmentValue(pageName + "." + fragmentName, "", nodeFragment.InnerText);
			}
		}
	}
}

