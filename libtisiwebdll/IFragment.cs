using System.Collections.Generic;

public interface IFragment
{
    string Name { get; set; }
    IFragmentSubsets Subsets { get; set; }
    void AddSubset(string subset, string value);
    bool ContainsSubset(string subset);
}
