using System.Reflection;

namespace ReflectionDiContainer.Container;

public interface ITypeScanner
{
    Assembly[] Assemblies { get; }
    IEnumerable<Type> Scan();
}