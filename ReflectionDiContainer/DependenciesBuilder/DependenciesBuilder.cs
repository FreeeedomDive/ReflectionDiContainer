using System.Reflection;
using ReflectionDiContainer.TypeScanner;

namespace ReflectionDiContainer.DependenciesBuilder;

public class DependenciesBuilder : IDependenciesBuilder
{
    public DependenciesBuilder(ITypeScanner typeScanner)
    {
        this.typeScanner = typeScanner;
        Roots = typeScanner.Scan();
        Dependencies = new Dictionary<Type, Type[]>();
        Implementations = new Dictionary<Type, Type>();
        BuildDependencyGraph();
    }

    private void BuildDependencyGraph()
    {
        var assemblies = typeScanner.Assemblies;
        ProcessTypes(assemblies, Roots, new Stack<Type>());
    }

    private void ProcessTypes(Assembly[] assemblies, IEnumerable<Type> types, Stack<Type> chain)
    {
        var depth = chain.Count;
        foreach (var type in types)
        {
            if (Implementations.TryGetValue(type, out var configuredType))
            {
                if (chain.Contains(type))
                {
                    throw new InvalidOperationException($"Loop detected: {string.Join(" -> ", chain.Reverse().Select(x => x.Name))} -> {type.Name}");
                }
                Console.WriteLine($"{new string('\t', depth)}Reused {type.Name} - {configuredType.Name}");
                continue;
            }

            var implementationTypes = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface)
                .ToArray();
            if (implementationTypes.Length != 1)
            {
                throw new InvalidOperationException($"Type {type} must have exactly one implementation.");
            }

            var implementationType = implementationTypes.Single();
            Implementations[type] = implementationType;
            Console.WriteLine($"{new string('\t', depth)}Created {type.Name} - {implementationType.Name}");

            var constructors = implementationType.GetConstructors();
            if (constructors.Length > 1)
            {
                throw new InvalidOperationException($"Type {implementationType} must have zero or one constructor.");
            }

            var constructor = constructors.First();
            var parameters = constructor.GetParameters().Select(p => p.ParameterType).ToArray();
            Dependencies[implementationType] = parameters;
            chain.Push(type);
            ProcessTypes(assemblies, parameters, chain);
            chain.Pop();
        }
    }

    public IEnumerable<Type> Roots { get; }
    public IDictionary<Type, Type[]> Dependencies { get; }
    public IDictionary<Type, Type> Implementations { get; }

    private readonly ITypeScanner typeScanner;
}