using ReflectionDiContainer.Models;

namespace ReflectionDiContainer.Tests;

public class Case2
{
    public interface IType8
    {
    }

    public class Type8 : IType8
    {
        public Type8(IType4 type4)
        {
        }
    }

    public interface IType7
    {
    }

    public class Type7 : IType7
    {
        public Type7(IType8 type8)
        {
        }
    }

    public interface IType6
    {
    }

    public class Type6 : IType6
    {
    }

    public interface IType5
    {
    }

    public class Type5 : IType5
    {
    }

    public interface IType4
    {
    }

    public class Type4 : IType4
    {
        public Type4(IType6 type6, IType7 type7)
        {
        }
    }

    public interface IType3
    {
    }

    public class Type3 : IType3
    {
    }

    public interface IType2
    {
    }

    public class Type2 : IType2
    {
        public Type2(IType5 type5, IType6 type6)
        {
        }
    }

    public interface IType1 : IEntryPoint
    {
    }

    public class Type1 : IType1
    {
        public Type1(IType2 type2, IType3 type3, IType4 type4)
        {
        }
    }
}