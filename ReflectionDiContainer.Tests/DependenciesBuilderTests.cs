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
        var action = () => dependencyBuilder.BuildDependencies();
        action.Should().NotThrow();
    }
}