using System.Reflection;

namespace Application;

public static class AssemblyReference
{
    public static Assembly GetAssembly() => typeof(AssemblyReference).Assembly;
}