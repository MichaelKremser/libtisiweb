using System;
using System.Xml;
using NUnit.Framework;
using mkcs.libtisiweb;

namespace mkcs.libtisiwebtest {

	public class XmlFragmentRepositoryReaderTest {

		public XmlFragmentRepositoryReaderTest () {
		}

		private IFragmentRepository _FragmentRepository = new FragmentRepository();
		private IFragmentRepositoryReader<XmlDocument> UUT = new XmlFragmentRepositoryReader();

		[Test()]
		public void T1_TestReadXmlRepository() {
			XmlDocument doc = new XmlDocument();
			doc.LoadXml("<pages><page name=\"start\"><fragment name=\"title\">Start</fragment><fragment name=\"header\"><subset lang=\"en\">This is the start page.</subset><subset lang=\"de\">Das ist die Startseite.</subset></fragment></page></pages>");
			UUT.ReadFragmentRepository(doc, _FragmentRepository);
		}

		[Test()]
		public void T2_TestGetFragmentValueWithoutSubset() {
			string fragmentValue = _FragmentRepository.GetFragmentValue("start.title", "en");
			Assert.AreEqual("Start", fragmentValue);
		}
	}
}

