namespace NewStackPlayground.Gateway.DependencyInjection
{
    public interface IDependencyResolver
    {
        TDependency Resolve<TDependency>(
            bool returnNullIfNotRegistered = false)
            where TDependency : class;
    }
}
