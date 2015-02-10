using System;
using System.Collections.Generic;

namespace mkcs.libtisiweb
{
	public interface IFragment
	{
		string Name { get; set; }
		FragmentSubsets Subsets { get; set; }
		void AddSubset (string subset, string value);
	}

	public class Fragment : IFragment
	{
		public Fragment()
		{
			Subsets = new FragmentSubsets(this);
		}

		public string Name { get; set; }

		public FragmentSubsets Subsets { get; set; }

		public void AddSubset(string subset, string value)
		{
			Subsets.Add (subset, value);
		}

		public override string ToString ()
		{
			return string.Format("[Fragment: Name={0}, Subsets={1}]", Name, Subsets.Count);
		}
	}

	public class FragmentSubsets : Dictionary<string, string>
	{
		public FragmentSubsets() { }

		public FragmentSubsets(IFragment OwningFragment) : this()
		{
			this.OwningFragment = OwningFragment;
		}

		public Fragment OwningFragment { get; private set; }

		public override string ToString ()
		{
			return string.Format ("[FragmentSubsets]{0}/{1}", (OwningFragment == null ? "" : OwningFragment), this.Count);
		}
	}
}