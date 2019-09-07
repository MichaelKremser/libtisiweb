using NUnit.Framework;
using mkcs.libtisiweb;
using libtisiwebdll.Factory;

namespace mkcs.libtisiwebtest
{

    [TestFixture()]
	public class FragmentTests {

        private IFragment GetTestFragment()
        {
            var factory = DefaultFactory.GetFactory();
            var fragment = Fragment.CreateFragment(factory);
            fragment.Name = "test";
            fragment.Subsets.Add("s1", "foo");
            fragment.Subsets.Add("s2", "bar");
            return fragment;
        }

		[Test()]
		public void Fragment_Name() {
			var fragment = GetTestFragment();
			Assert.AreEqual("test", fragment.Name);
		}

		[Test()]
		public void Fragment_Subsets() {
			var fragment = GetTestFragment();
			Assert.IsTrue(fragment.ContainsSubset("s1"));
			Assert.AreEqual("foo", fragment.Subsets["s1"]);
		}
		
		[Test()]
		public void Fragment_Subsets_OwningFragment() {
			var fragment = GetTestFragment();
			Assert.AreEqual(fragment, fragment.Subsets.OwningFragment);
        }

        [Test()]
        public void Fragment_ToString_Test()
        {
            var fragment = GetTestFragment();
            var actualValue = fragment.ToString();
            Assert.AreEqual("[Fragment: Name=test, Subsets=2]", actualValue);
        }

    }
}