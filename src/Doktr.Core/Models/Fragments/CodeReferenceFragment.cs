using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class CodeReferenceFragment : DocumentationFragment
{
    public CodeReferenceFragment(CodeReference codeReference)
    {
        CodeReference = codeReference;
    }

    public CodeReference CodeReference { get; set; }
    public DocumentationFragmentCollection? Replacement { get; set; }

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitCodeReference(this);

    public override CodeReferenceFragment Clone() => new(CodeReference)
    {
        Replacement = Replacement?.Clone()
    };
}