using mkcs.libtisiweb;

namespace libtisiwebdll.Factory
{
    public class DefaultFactory : IFactory
    {
        internal DefaultFactory()
        {
        }

        public static IFactory GetFactory()
        {
            return new DefaultFactory();
        }

        public IFragmentSubsets CreateFragmentSubsets(IFragment owningFragment)
        {
            return new FragmentSubsets(owningFragment);
        }
    }
}
