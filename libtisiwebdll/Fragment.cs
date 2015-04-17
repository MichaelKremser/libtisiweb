using System;

namespace mkcs.libtisiweb
{
	public interface IFragment
	{
		string Name { get; set; }
		FragmentSubsets Subsets { get; set; }
		void AddSubset (string subset, string value);
		bool ContainsSubset(string subset);
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

		public bool ContainsSubset(string subset)
		{
			return Subsets.ContainsKey (subset);
		}

		public override string ToString ()
		{
			return string.Format("[Fragment: Name={0}, Subsets={1}]", Name, Subsets.Count);
		}
	}

}