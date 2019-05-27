using System;
using System.Xml;
using NUnit.Framework;
using mkcs.libtisiweb;
using System.Diagnostics;
using libtisiwebdll.Factory;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2015
 * 
 * This is free software.
 * License: MIT
*/

namespace mkcs.libtisiwebtest {

	public class XmlFragmentRepositoryReaderTest {

		public XmlFragmentRepositoryReaderTest () {
            factory = DefaultFactory.GetFactory();
            _FragmentRepository = new FragmentRepository(factory);
        }

        private IFactory factory;
        private IFragmentRepository _FragmentRepository;
		private IFragmentRepositoryReader<XmlDocument> UUT = new XmlFragmentRepositoryReader();

		[Test()]
		public void T1_TestReadXmlRepository() {
			var doc = new XmlDocument();
			doc.LoadXml("<pages><page name=\"start\"><fragment name=\"title\">Start</fragment><fragment name=\"header\"><subset lang=\"en\">This is the start page.</subset><subset lang=\"de\">Das ist die Startseite.</subset></fragment></page></pages>");
			UUT.ReadFragmentRepository(doc, _FragmentRepository);
		}

		[Test()]
		public void T2_TestGetFragmentValueWithoutSubset() {
			string fragmentValue = _FragmentRepository.GetFragmentValue("start.title", "en");
			Assert.AreEqual("Start", fragmentValue);
		}

		[Test()]
		public void T3_TestReadXmlRepositoryFromFile() {
			var doc = new XmlDocument();
			doc.Load("/media/Daten1/Files/Great/Web/tobias-jana.mkcs.at/data/baby2008.xml");
			UUT.ReadFragmentRepository(doc, _FragmentRepository);
		}
		
		[Test()]
		public void T4_TestGetFragmentValueFromFile_1() {
			string fragmentValue = _FragmentRepository.GetFragmentValue("welcome.title", "en");
			Assert.AreEqual("Welcome", fragmentValue);
			fragmentValue = _FragmentRepository.GetFragmentValue("welcome.title", "de");
			Assert.AreEqual("Willkommen", fragmentValue);
			fragmentValue = _FragmentRepository.GetFragmentValue("welcome.title", "ru");
			Assert.AreEqual("Добро пожаловать", fragmentValue);
		}
	}
}

