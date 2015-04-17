using System;
using System.Xml;

namespace mkcs.libtisiweb {

	public class XmlFragmentRepositoryReader : IFragmentRepositoryReader<XmlDocument> {

		public XmlFragmentRepositoryReader () {
		}

		public void ReadFragmentRepository(XmlDocument doc, IFragmentRepository fragmentRepository) {
			ProcessRepositoryNodes(doc.SelectNodes("/pages/page"), fragmentRepository);
		}

		public void ProcessRepositoryNodes(XmlNodeList nodes, IFragmentRepository fragmentRepository) {
			if (nodes == null || nodes.Count == 0)
				return;
			//Trace.WriteLine("ProcessRepositoryNodes(" + nodes.Count.ToString() + ")");
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
							fragmentRepository.SetFragmentValue(pageName + "." + fragmentName, subsetId, nodeSubset.InnerText);
						}
					}
					else {
						// No, so add the fragment's node text
						fragmentRepository.SetFragmentValue(pageName + "." + fragmentName, "", nodeFragment.InnerText);
					}
				}
				ProcessRepositoryNodes(node.SelectNodes("page"), fragmentRepository);
			}
		}

	}
}

