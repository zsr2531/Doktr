namespace Doktr.Core.Models;

public class FinalizerDocumentation : MemberDocumentation
{
    public FinalizerDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitFinalizer(this);

    public override FinalizerDocumentation Clone() => new(Name, Visibility);
}