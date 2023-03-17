using FluentAssertions;

namespace ReflectionDiContainer.Tests;

public class DependenciesBuilderTests
{
    [Test]
    public void Case()
    {
        var typeScanner = new TypeScanner.TypeScanner();
        var action = () => new DependenciesBuilder.DependenciesBuilder(typeScanner);
        action.Should().NotThrow();
    }
}