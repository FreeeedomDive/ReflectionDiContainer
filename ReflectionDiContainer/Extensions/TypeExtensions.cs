using Microsoft.AspNetCore.Mvc;
using ReflectionDiContainer.Models;

namespace ReflectionDiContainer.Extensions;

internal static class TypeExtensions
{
    public static bool IsController(this Type type)
    {
        return typeof(ControllerBase).IsAssignableFrom(type);
    }

    public static bool IsEntryPoint(this Type type)
    {
        return typeof(IEntryPoint).IsAssignableFrom(type) && type != typeof(IEntryPoint);
    }
}