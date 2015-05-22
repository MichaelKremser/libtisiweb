using System;
using System.Xml;

namespace mkcs.libtisiweb
{
	public class Xml
	{
		public Xml () { }

		public static string GetAttributeValueDefensive(XmlNode node, string attributeName, string defaultValue = "") {
			XmlAttribute attribute = (XmlAttribute)node.Attributes.GetNamedItem(attributeName);
			if (attribute != null)
				return attribute.Value;
			else
				return defaultValue;
		}
	}
}

