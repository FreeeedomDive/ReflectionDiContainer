using Microsoft.Extensions.DependencyInjection;
using ReflectionDiContainer.Container;

namespace ReflectionDiContainer.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterDependencies(this IServiceCollection services)
    {
        var typeScanner = new TypeScanner();
        var dependenciesBuilder = new DependenciesBuilder(typeScanner);
        dependenciesBuilder.BuildDependencies();
        foreach (var rootType in dependenciesBuilder.Roots)
        {
            RegisterType(
                services,
                rootType,
                dependenciesBuilder.Dependencies,
                dependenciesBuilder.Implementations,
                new HashSet<Type>()
            );
        }
    }

    private static void RegisterType(
        IServiceCollection services,
        Type serviceType,
        IDictionary<Type, Type[]> dependencies,
        IDictionary<Type, Type> implementations,
        ISet<Type> processing
    )
    {
        if (processing.Contains(serviceType))
        {
            return;
        }

        processing.Add(serviceType);

        if (!implementations.TryGetValue(serviceType, out var implementationType))
        {
            throw new InvalidOperationException($"No implementation found for type {serviceType.FullName}.");
        }

        var dependenciesTypes = dependencies[implementationType];
        foreach (var dependencyType in dependenciesTypes)
        {
            RegisterType(services, dependencyType, dependencies, implementations, processing);
        }

        services.AddTransient(serviceType, implementationType);

        processing.Remove(serviceType);
    }
}