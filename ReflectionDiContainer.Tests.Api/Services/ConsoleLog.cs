namespace ReflectionDiContainer.Tests.Api.Services;

public class ConsoleLog<T> : ILog<T>
{
    public void Info(string message)
    {
        Console.WriteLine($"{typeof(T).Name}: {message}");
    }
}