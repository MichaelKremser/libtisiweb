using System;
using System.Collections.Generic;

namespace mkcs.libtisiweb
{
	public class Fragment
	{
		public string Name { get; set; }

		public FragmentSubsets Subsets { get; set; }

		public void AddSubset(string subset, string value)
		{
			Subsets.Add (subset, value);
		}
	}

	public class FragmentSubsets : Dictionary<string, string>
	{
		public Fragment OwningFragment { get; set; }

		public override string ToString ()
		{
			return string.Format ("[FragmentSubsets]{0}{1}", (OwningFragment == null ? "" : OwningFragment), this.Count);
		}
	}
}