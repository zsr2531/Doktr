using System.Linq;
using System.Reflection;
using AsmResolver.DotNet;

namespace Doktr.Lifters.AsmResolver.Tests;

public class ModuleFixture<T>
{
    public ModuleFixture()
    {
        string assembly = typeof(T).Assembly.Location;
        Module = ModuleDefinition.FromFile(assembly);
    }

    public ModuleDefinition Module { get; }

    public IMemberDefinition GetMember(MemberInfo memberInfo)
    {
        int needle = memberInfo.MetadataToken;
        return Module.GetAllTypes()
                     .SelectMany(t =>
                         t.Events.Concat<IMemberDefinition>(t.Fields).Concat(t.Properties).Concat(t.Methods))
                     .First(m => m.MetadataToken == needle);
    }
}