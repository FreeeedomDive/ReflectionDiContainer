namespace ReflectionDiContainer.Models;

public class DependencyTree
{
    public IEnumerable<Type> Roots { get; init; }
    public IDictionary<Type, Type[]> Dependencies { get; init; }
    public IDictionary<Type, Type> Implementations { get; init; }
    public IDictionary<Type, object> Instances { get; init; }
    public HashSet<Type> Skip { get; init; }
}