namespace ReflectionDiContainer.DependenciesBuilder;

public interface IDependenciesBuilder
{
    IEnumerable<Type> Roots { get; }
    IDictionary<Type, Type[]> Dependencies { get; }
    IDictionary<Type, Type> Implementations { get; }
}