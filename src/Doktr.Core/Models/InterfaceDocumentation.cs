namespace Doktr.Core.Models;

public class InterfaceDocumentation : CommonTypeCharacteristics
{
    public InterfaceDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }


    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitInterface(this);

    public override InterfaceDocumentation Clone()
    {
        var clone = new InterfaceDocumentation(Name, Visibility);
        CopyDocumentationTo(clone);

        return clone;
    }
}