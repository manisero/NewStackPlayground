using SimpleInjector;

namespace NewStackPlayground.Gateway.DependencyInjection
{
    public class SimpleInjectorDependencyResolver : IDependencyResolver
    {
        private readonly Container _container;

        public SimpleInjectorDependencyResolver(
            Container container)
        {
            _container = container;
        }

        public TDependency Resolve<TDependency>(
            bool returnNullIfNotRegistered = false)
            where TDependency : class
        {
            if (returnNullIfNotRegistered)
            {
                var registration = _container.GetRegistration(typeof(TDependency));

                if (registration == null)
                {
                    return null;
                }
            }

            return _container.GetInstance<TDependency>();
        }
    }
}
