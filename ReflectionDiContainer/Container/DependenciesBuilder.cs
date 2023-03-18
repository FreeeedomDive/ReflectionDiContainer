using System.Text;
using ReflectionDiContainer.Models;

namespace ReflectionDiContainer.Container;

public class DependenciesBuilder : IDependenciesBuilder
{
    public DependenciesBuilder(ITypeScanner typeScanner)
    {
        this.typeScanner = typeScanner;
        dependencyTree = new DependencyTree
        {
            Roots = typeScanner.Scan(),
            Dependencies = new Dictionary<Type, Type[]>(),
            Implementations = new Dictionary<Type, Type>(),
            Instances = new Dictionary<Type, object>(),
            Skip = new HashSet<Type>(),
        };
    }

    public DependenciesBuilder Skip<T>()
    {
        dependencyTree.Skip.Add(typeof(T));
        return this;
    }

    public DependenciesBuilder Register<TInterface, TImplementation>()
    {
        dependencyTree.Implementations[typeof(TInterface)] = typeof(TImplementation);
        return this;
    }

    public DependenciesBuilder Register<TInterface>(object instance)
    {
        dependencyTree.Instances[typeof(TInterface)] = instance;
        return this;
    }

    public DependencyTree Build()
    {
        var types = typeScanner.Assemblies.SelectMany(x => x.GetTypes()).ToArray();
        ProcessTypes(types, dependencyTree.Roots, new Stack<Type>());
        return dependencyTree;
    }

    private void ProcessTypes(Type[] assembliesTypes, IEnumerable<Type> types, Stack<Type> chain)
    {
        var depth = chain.Count;
        foreach (var type in types)
        {
            if (dependencyTree.Skip.Contains(type))
            {
                Console.WriteLine(SkipTypeToString(type, depth));
                continue;
            }

            if (dependencyTree.Implementations.TryGetValue(type, out var configuredType))
            {
                if (chain.Contains(type))
                {
                    throw new InvalidOperationException(
                        $"Loop detected: {string.Join(" -> ", chain.Reverse().Select(x => x.Name))} -> {type.Name}");
                }

                Console.WriteLine(TypeToString(type, configuredType, depth, false));
                continue;
            }

            if (dependencyTree.Instances.TryGetValue(type, out _))
            {
                Console.WriteLine(InstanceToString(type, depth));
                continue;
            }

            var (interfaceType, implementationType) = CreateImplementation(assembliesTypes, type, depth);
            dependencyTree.Implementations[interfaceType] = implementationType;

            var constructors = implementationType.GetConstructors();
            if (constructors.Length > 1)
            {
                throw new InvalidOperationException($"Type {implementationType} must have zero or one constructor.");
            }

            var constructor = constructors.First();
            var parameters = constructor.GetParameters().Select(p => p.ParameterType).ToArray();
            dependencyTree.Dependencies[implementationType] = parameters;
            chain.Push(type);
            ProcessTypes(assembliesTypes, parameters, chain);
            chain.Pop();
        }
    }

    private static (Type, Type) CreateImplementation(IEnumerable<Type> assembliesTypes, Type type, int depth)
    {
        if (!type.IsGenericType)
        {
            var implementationTypes = assembliesTypes
                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface)
                .ToArray();
            if (implementationTypes.Length != 1)
            {
                throw new InvalidOperationException($"Type {type} must have exactly one implementation.");
            }

            var implementationType = implementationTypes.Single();
            Console.WriteLine(TypeToString(type, implementationType, depth));
            return (type, implementationType);
        }

        var genericTypeDefinition = type.GetGenericTypeDefinition();
        var typeArguments = type.GetGenericArguments();
        var genericImplementationTypes = assembliesTypes
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType
                && i.GetGenericTypeDefinition() == genericTypeDefinition)
            ).ToArray();
        if (genericImplementationTypes.Length != 1)
        {
            throw new InvalidOperationException($"Generic type {type} must have exactly one generic implementation.");
        }

        var genericImplementation = genericImplementationTypes.Single();

        var genericInterfaceType = genericTypeDefinition.MakeGenericType(typeArguments);
        var genericImplementationType = genericImplementation.MakeGenericType(typeArguments);
        Console.WriteLine(TypeToString(genericInterfaceType, genericImplementationType, depth));
        return (genericInterfaceType, genericImplementationType);
    }

    private static string TypeToString(Type interfaceType, Type implementationType, int depth, bool isCreated = true)
    {
        var sb = CreateStringBuilderWithShift(depth);
        sb.Append(isCreated ? "Created " : "Reused ");
        if (!interfaceType.IsGenericType)
        {
            sb.Append($"{interfaceType.Name} - {implementationType.Name}");
        }
        else
        {
            var typeArguments = interfaceType.GetGenericArguments();
            var stringGenericTypes = string.Join(", ", typeArguments.Select(x => x.Name));
            sb.Append($"{interfaceType.Name}<{stringGenericTypes}> - {implementationType.Name}<{stringGenericTypes}>");
        }

        return sb.ToString();
    }

    private static string InstanceToString(Type type, int depth)
    {
        return CreateStringBuilderWithShift(depth)
            .Append($"Reused {type.Name} - instance")
            .ToString();
    }

    private static string SkipTypeToString(Type type, int depth)
    {
        return CreateStringBuilderWithShift(depth)
            .Append($"Skipped {type.Name}")
            .ToString();
    }

    private static StringBuilder CreateStringBuilderWithShift(int count)
    {
        return new StringBuilder(string.Join("", Enumerable.Repeat("  ", count)));
    }

    private readonly ITypeScanner typeScanner;
    private readonly DependencyTree dependencyTree;
}