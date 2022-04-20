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
        var clone = new IndexerDocumentation(Name, Visibility, Type.Clone());
            
        CopyDocumentationTo(clone);
        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is not IndexerDocumentation otherIndexer)
            throw new ArgumentException("Cannot copy documentation to a non-indexer member.", nameof(other));
        
        otherIndexer.Parameters = Parameters.Clone();
        base.CopyDocumentationTo(other);
    }
}