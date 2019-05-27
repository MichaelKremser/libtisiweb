using System;
using System.Collections.Generic;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2019
 * 
 * This is free software.
 * License: MIT
*/

/*
 * 20150417: put to seperate file
*/

namespace mkcs.libtisiweb
{
	public class FragmentSubsets : Dictionary<string, string>, IFragmentSubsets
    {
		public FragmentSubsets() { }

		public FragmentSubsets(IFragment OwningFragment) : this()
		{
			this.OwningFragment = OwningFragment;
		}

		public IFragment OwningFragment { get; set; }

		public override string ToString ()
		{
			return string.Format ("[FragmentSubsets]{0}/{1}", (OwningFragment == null ? "" : OwningFragment.ToString()), this.Count);
		}
	}
}
