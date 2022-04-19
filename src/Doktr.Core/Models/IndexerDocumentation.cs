using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class IndexerDocumentation : PropertyDocumentation, IHasParameters
{
    public IndexerDocumentation(string name, MemberVisibility visibility, TypeSignature type)
        : base(name, visibility, type)
    {
    }

    public ParameterSegmentCollection Parameters { get; set; } = new();

    public override IndexerDocumentation Clone()
    {
        var clone = new IndexerDocumentation(Name, Visibility, Type.Clone())
        {
            Parameters = Parameters.Clone()
        };
        
        CopyDocumentationTo(clone);
        return clone;
    }
}