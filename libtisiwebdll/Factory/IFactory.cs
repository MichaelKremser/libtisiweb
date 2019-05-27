using System;
namespace libtisiwebdll.Factory
{
    public interface IFactory
    {
        IFragmentSubsets CreateFragmentSubsets(IFragment owningFragment);
    }
}
