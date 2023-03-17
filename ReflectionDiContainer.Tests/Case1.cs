using ReflectionDiContainer.Models;

namespace ReflectionDiContainer.Tests;

public class Case1
{
    public interface IService
    {
    }

    public class Service : IService
    {
        public Service(ISecondOtherService secondOtherService)
        {
        }
    }

    public interface IController
    {
    }

    public class Controller : IController
    {
        public Controller(IService service)
        {
        }
    }

    public interface IOtherService
    {
    }

    public class OtherService : IOtherService
    {
        public OtherService(IController controller)
        {
        }
    }

    public interface ISecondOtherService
    {
    }

    public class SecondOtherService : ISecondOtherService
    {
    }

    public interface IMyEntryPoint : IEntryPoint
    {
    }

    public class MyEntryPoint : IMyEntryPoint
    {
        public MyEntryPoint(IOtherService otherService, ISecondOtherService secondOtherService)
        {
        }
    }
}