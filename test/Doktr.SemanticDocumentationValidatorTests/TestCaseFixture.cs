using System.Collections.Immutable;
using System.Linq;
using AsmResolver.DotNet;
using Doktr.Dependencies;

namespace Doktr.SemanticDocumentationValidatorTests;

public class TestCaseFixture<T>
{
    public TestCaseFixture()
    {
        string location = typeof(T).Assembly.Location;
        var module = ModuleDefinition.FromFile(location);
        int token = typeof(T).MetadataToken;
        TestType = module.GetAllTypes().Single(t => t.MetadataToken == token);

        var root = new DependencyNode(module.Assembly);
        var type = new DependencyNode(TestType, root);
        DependencyGraph = new DependencyGraph(ImmutableDictionary<IFullNameProvider, DependencyNode>.Empty,
            ImmutableArray.Create(root));
    }
    
    public DependencyGraph DependencyGraph
    {
        get;
    }
    
    public TypeDefinition TestType
    {
        get;
    }
}