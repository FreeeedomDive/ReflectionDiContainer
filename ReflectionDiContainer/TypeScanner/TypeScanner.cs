using System.Reflection;
using ReflectionDiContainer.Extensions;

namespace ReflectionDiContainer.TypeScanner;

public class TypeScanner : ITypeScanner
{
    public TypeScanner(Assembly[]? assemblies = null)
    {
        Assemblies = assemblies ?? AppDomain.CurrentDomain.GetAssemblies();
    }

    public IEnumerable<Type> Scan()
    {
        return Assemblies.SelectMany(assembly =>
            assembly.GetTypes().Where(type =>
                (type.IsEntryPoint() && type.IsInterface)
                || (type.IsController())
            )
        );
    }

    public Assembly[] Assemblies { get; }
}