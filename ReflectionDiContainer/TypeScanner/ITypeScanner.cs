using System.Reflection;

namespace ReflectionDiContainer.TypeScanner;

public interface ITypeScanner
{
    Assembly[] Assemblies { get; }
    IEnumerable<Type> Scan();
}