using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class CodeReferenceFragment : IDocumentationFragment
{
    public CodeReferenceFragment(CodeReference codeReference)
    {
        CodeReference = codeReference;
    }

    public CodeReference CodeReference { get; set; }
    public DocumentationFragmentCollection? ReplacementText { get; set; }

    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitCodeReference(this);
}