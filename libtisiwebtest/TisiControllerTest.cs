using NUnit.Framework;
using System;
using System.Xml;
using mkcs.libtisiweb;

namespace mkcs.libtisiwebtest {

	public class TisiControllerTest : TisiController {

		public TisiControllerTest() {
		}

		public IFragmentRepository GetFragmentRepository() {
			return this.fragmentRepository;
		}
	}

	[TestFixture()]
	public class TestFixtureTisiController {
		
		TisiControllerTest tisiController = new TisiControllerTest();

	}
}