using FluentAssertions;

namespace ReflectionDiContainer.Tests;

public class DependenciesBuilderTests
{
    [Test]
    public void Case()
    {
        var typeScanner = new TypeScanner.TypeScanner();
        var dependencyBuilder = new DependenciesBuilder.DependenciesBuilder(typeScanner);
        var action = () => dependencyBuilder.BuildDependencies();
        action.Should().NotThrow();
    }
}