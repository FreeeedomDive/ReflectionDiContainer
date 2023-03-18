using ReflectionDiContainer.Models;

namespace ReflectionDiContainer.Container;

public interface IDependenciesBuilder
{
    DependenciesBuilder Skip<T>();
    DependenciesBuilder Register<TInterface, TImplementation>();
    DependenciesBuilder Register<TInterface>(object instance);
    DependencyTree Build();
}