using libtisiwebdll.Factory;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2019
 * 
 * This is free software.
 * License: MIT
*/

namespace mkcs.libtisiweb
{
	/// <summary>
    /// A fragment is a particle of a page. It contains subsets which have an identifier.
    /// A subset defines the result for a condition which is expressed by the identifier. For example, the condition might be a language.
    /// </summary>
    public class Fragment : IFragment
	{
		internal Fragment()
		{
		}

        public static IFragment CreateFragment(IFactory factory)
        {
            var fragment = new Fragment();
            fragment.Subsets = factory.CreateFragmentSubsets(fragment);
            return fragment;
        }

		public string Name { get; set; }

		public IFragmentSubsets Subsets { get; set; }

		public void AddSubset(string subset, string value)
		{
			Subsets.Add (subset, value);
		}

		public bool ContainsSubset(string subset)
		{
			return Subsets.ContainsKey (subset);
		}

		public override string ToString ()
		{
			return string.Format($"[{nameof(Fragment)}: Name={Name}, Subsets={Subsets.Count}]");
		}
	}

}