using System;
using System.Xml;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2015
 * 
 * This is free software.
 * License: MIT
*/

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

