using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public class InterfaceDocumentation : TypeDocumentation, IHasTypeParameters, IHasCommonTypeCharacteristics
{
    public InterfaceDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }


    public TypeParameterDocumentationCollection TypeParameters { get; set; } = new();
    public TypeSignatureCollection Interfaces { get; set; } = new();
    public MemberCollection<EventDocumentation> Events { get; set; } = new();
    public MemberCollection<IndexerDocumentation> Indexers { get; set; } = new();
    public MemberCollection<PropertyDocumentation> Properties { get; set; } = new();
    public MemberCollection<MethodDocumentation> Methods { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitInterface(this);

    public override InterfaceDocumentation Clone()
    {
        var clone = new InterfaceDocumentation(Name, Visibility);
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is InterfaceDocumentation otherInterface)
        {
            otherInterface.TypeParameters = TypeParameters.Clone();
            otherInterface.Interfaces = Interfaces.Clone();
            otherInterface.Events = Events.Clone();
            otherInterface.Indexers = Indexers.Clone();
            otherInterface.Properties = Properties.Clone();
            otherInterface.Methods = Methods.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}