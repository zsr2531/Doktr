using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class TypeDocumentation : MemberDocumentation, IHasTypeParameters, IHasExtensionMethods
{
    protected TypeDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public TypeParameterSegmentCollection TypeParameters { get; set; } = new();
    public ExtensionMethodCollection ExtensionMethods { get; set; } = new();
}