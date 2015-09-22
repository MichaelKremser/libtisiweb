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
			if (nodes == null || nodes.Count == 0)
				return;
			Console.WriteLine("ProcessRepositoryNodes(" + nodes.Count.ToString() + ")");
			string pageName = "", fragmentName = "", subsetId = "";
			foreach (XmlNode node in nodes) {
				pageName = Xml.GetAttributeValueDefensive(node, "name"); // node.Attributes["name"].Value;
				if (pageName.Length == 0) {
					Console.WriteLine("\tSkipping unnamed page");
				}
				else {
					Console.WriteLine("\tProcessing page node '" + pageName + "'");
					foreach (XmlNode nodeFragment in node.SelectNodes(FragmentNodeName)) {
						fragmentName = Xml.GetAttributeValueDefensive(nodeFragment, "name"); // nodeFragment.Attributes["name"].Value;
						if (fragmentName.Length == 0) {
							Console.WriteLine("\tSkipping unnamed fragment");
						}
						else {
							Console.WriteLine("\tProcessing fragment node '" + fragmentName + "'");
							XmlNodeList subsetNodes = nodeFragment.SelectNodes(SubsetNodeName);
							// Does this fragment have subsets?
							if (subsetNodes.Count > 0) {
								int subsetNodePtr = 0;
								// Yes, so let's add every subset and its text
								foreach (XmlNode nodeSubset in subsetNodes) {
									Console.WriteLine("\tProcessing subset node #'" + (++subsetNodePtr).ToString() + "'");
									subsetId = Xml.GetAttributeValueDefensive(nodeSubset, SubsetNameAttribute); // nodeSubset.Attributes[SubsetNameAttribute].Value;
									if (subsetId.Length == 0) {
										Console.WriteLine("\tNot adding subset without name");
									}
									else {
										fragmentRepository.SetFragmentValue(pageName + "." + fragmentName, subsetId, nodeSubset.InnerText);
									}
								}
							}
							else {
								// No, so add the fragment's node text
								fragmentRepository.SetFragmentValue(pageName + "." + fragmentName, "", nodeFragment.InnerText);
							}
						}
					}
					ProcessRepositoryNodes(node.SelectNodes(PageNodeName), fragmentRepository);
				}
			}
			Console.WriteLine("ProcessRepositoryNodes exit");
		}

	}
}

