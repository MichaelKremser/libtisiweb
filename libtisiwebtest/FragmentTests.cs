using NUnit.Framework;
using System;

namespace mkcs.libtisiwebtest {

	[TestFixture()]
	public class FragmentTests {

		[Test()]
		public void Fragment_Name() {
			var fragment = new mkcs.libtisiweb.Fragment();
			fragment.Name = "test";
			Assert.AreEqual("test", fragment.Name);
		}

		[Test()]
		public void Fragment_Subsets() {
			var fragment = new mkcs.libtisiweb.Fragment();
			fragment.Subsets.Add("s1", "foo");
			Assert.IsTrue(fragment.ContainsSubset("s1"));
			Assert.AreEqual("foo", fragment.Subsets["s1"]);
		}
		
		[Test()]
		public void Fragment_Subsets_OwningFragment() {
			var fragment = new mkcs.libtisiweb.Fragment();
			fragment.Subsets.Add("s1", "foo");
			Assert.AreEqual(fragment, fragment.Subsets.OwningFragment);
		}
	}
}