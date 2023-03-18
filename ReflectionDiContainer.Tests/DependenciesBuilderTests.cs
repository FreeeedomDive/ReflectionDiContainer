using FluentAssertions;
using ReflectionDiContainer.Container;

namespace ReflectionDiContainer.Tests;

public class DependenciesBuilderTests
{
    [Test]
    public void Case()
    {
        var typeScanner = new TypeScanner();
        var dependencyBuilder = new DependenciesBuilder(typeScanner);
        var action = () => dependencyBuilder
            .Skip<Case2.ISkipable>()
            .Register<Case2.ISingleton>(new Case2.Singleton())
            .Build();
        action.Should().NotThrow();
    }
}