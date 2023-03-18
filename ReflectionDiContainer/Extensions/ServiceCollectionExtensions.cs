using Microsoft.Extensions.DependencyInjection;
using ReflectionDiContainer.Container;
using ReflectionDiContainer.Models;

namespace ReflectionDiContainer.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterDependencies(this IServiceCollection services)
    {
        var typeScanner = new TypeScanner();
        var dependenciesBuilder = new DependenciesBuilder(typeScanner);
        var tree = dependenciesBuilder.Build();
        foreach (var rootType in tree.Roots)
        {
            RegisterType(services, rootType, tree, new HashSet<Type>());
        }
    }

    private static void RegisterType(
        IServiceCollection services,
        Type serviceType,
        DependencyTree tree,
        ISet<Type> processing
    )
    {
        if (tree.Skip.Contains(serviceType) || processing.Contains(serviceType))
        {
            return;
        }

        processing.Add(serviceType);

        if (tree.Implementations.TryGetValue(serviceType, out var implementationType))
        {
            var dependenciesTypes = tree.Dependencies[implementationType];
            foreach (var dependencyType in dependenciesTypes)
            {
                RegisterType(services, dependencyType, tree, processing);
            }

            services.AddTransient(serviceType, implementationType);
        }
        else if (tree.Instances.TryGetValue(serviceType, out var instance))
        {
            services.AddSingleton(serviceType, instance);
        }
        else
        {
            throw new InvalidOperationException($"No implementation found for type {serviceType.FullName}.");
        }

        processing.Remove(serviceType);
    }
}