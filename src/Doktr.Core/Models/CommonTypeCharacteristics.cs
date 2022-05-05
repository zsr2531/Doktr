using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class CommonTypeCharacteristics : TypeDocumentation, IHasTypeParameters
{
    protected CommonTypeCharacteristics(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public TypeParameterDocumentationCollection TypeParameters { get; set; } = new();
    public TypeSignatureCollection Interfaces { get; set; } = new();
    public MemberCollection<EventDocumentation> Events { get; set; } = new();
    public MemberCollection<IndexerDocumentation> Indexers { get; set; } = new();
    public MemberCollection<PropertyDocumentation> Properties { get; set; } = new();
    public MemberCollection<MethodDocumentation> Methods { get; set; } = new();

    public abstract override CommonTypeCharacteristics Clone();

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is CommonTypeCharacteristics otherType)
        {
            otherType.TypeParameters = TypeParameters.Clone();
            otherType.Interfaces = Interfaces.Clone();
            otherType.Events = Events.Clone();
            otherType.Indexers = Indexers.Clone();
            otherType.Properties = Properties.Clone();
            otherType.Methods = Methods.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}