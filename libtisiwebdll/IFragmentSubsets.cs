using System.Collections.Generic;

public interface IFragmentSubsets : IDictionary<string, string>
{
    IFragment OwningFragment { get; set; }
}