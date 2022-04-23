using System.Diagnostics.CodeAnalysis;

namespace Doktr.Core.Models;

public class FinalizerDocumentation : MemberDocumentation, IHasVirtual
{
    public FinalizerDocumentation(string name)
        : base(name, MemberVisibility.Protected)
    {
    }

    public bool IsAbstract => false;
    public bool IsSealed => false;
    public bool IsVirtual => false;
    public bool IsOverride => true;
    [NotNull]
    public CodeReference? Overrides => new("M:System.Object.Finalize()");

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitFinalizer(this);

    public override FinalizerDocumentation Clone() => new(Name);
}