namespace Doktr.Core.Models;

public class ExplicitImplementation : ICloneable
{
    public ExplicitImplementation(CodeReference declaration, MethodDocumentation implementation)
    {
        Declaration = declaration;
        Implementation = implementation;
    }

    public CodeReference Declaration { get; set; }
    public MethodDocumentation Implementation { get; set; }

    public ExplicitImplementation Clone() => new(Declaration, Implementation);

    object ICloneable.Clone() => Clone();
}