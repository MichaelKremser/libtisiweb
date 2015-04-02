using NUnit.Framework;
using System;
using System.Xml;
using mkcs.libtisiweb;

namespace libtisiwebtest {

	public class TisiControllerTest : TisiController {

		public TisiControllerTest() {
		}
	}

	[TestFixture()]
	public class TestFixtureTisiController {
		
		TisiControllerTest tisiController = new TisiControllerTest();

		[Test()]
		public void TestReadXmlRepository() {
			XmlDocument doc = new XmlDocument();
			doc.LoadXml("<pages><page name=\"start\"><fragment name=\"title\">Start</fragment><fragment name=\"header\"><subset lang=\"en\">This is the start page.</subset><subset lang=\"de\">Das ist die Startseite.</subset></fragment></page></pages>");
			tisiController.ReadXmlRepository(doc);
		}

		[Test()]
		public void TestGetFragmentValueWithoutSubset() {
			string fragmentValue = tisiController.GetFragmentValue("start.title", "en");
			Assert.AreEqual("Start", fragmentValue);
		}
	}
}