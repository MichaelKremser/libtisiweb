using System;
using System.Collections.Generic;

namespace mkcs.libtisiweb
{
	public class FragmentSubsets : Dictionary<string, string>
	{
		public FragmentSubsets() { }

		public FragmentSubsets(IFragment OwningFragment) : this()
		{
			this.OwningFragment = OwningFragment;
		}

		public IFragment OwningFragment { get; private set; }

		public override string ToString ()
		{
			return string.Format ("[FragmentSubsets]{0}/{1}", (OwningFragment == null ? "" : OwningFragment.ToString()), this.Count);
		}
	}
}
