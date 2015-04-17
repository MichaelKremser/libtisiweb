using System;
using mkcs.libtisiweb;
using NUnit.Framework;

namespace libtisiwebtest {

	public class FragmentRepositoryTest {

		public FragmentRepositoryTest () {
			UUT.DefaultSubset = "en";
		}

		private FragmentRepository UUT = new FragmentRepository();

		[Test()]
		public void T1_SetFragmentValue() {
			UUT.SetFragmentValue("navigation.about", "en", "About");
			UUT.SetFragmentValue("navigation.about", "de", "Über");
			UUT.SetFragmentValue("welcome.text", "en", "Welcome on this page");
			UUT.SetFragmentValue("general.copyright", "", "(C) me, 2015"); // language independent
			Assert.AreEqual(3, UUT.Count);
		}

		[Test()]
		public void T2_GetFragmentValue() {
			Assert.AreEqual("About", UUT.GetFragmentValue("navigation.about", "en"));
			Assert.AreEqual("Über", UUT.GetFragmentValue("navigation.about", "de"));
			Assert.AreEqual("Welcome on this page", UUT.GetFragmentValue("welcome.text", "en"));
		}

		[Test()]
		public void T3_GetFragmentValue() {
			Assert.AreEqual("Welcome on this page", UUT.GetFragmentValue("welcome.text", "de")); // test fall back to DefaultSubset
		}
		
		[Test()]
		public void T4_GetFragmentValue() {
			// Test if language independent queries work
			Assert.AreEqual("(C) me, 2015", UUT.GetFragmentValue("general.copyright", "de"));
			Assert.AreEqual("(C) me, 2015", UUT.GetFragmentValue("general.copyright", "en"));
		}
	}
}