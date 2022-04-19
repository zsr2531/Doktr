using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class CodeFragment : DocumentationFragment
{
    public DocumentationFragmentCollection Content { get; set; } = new();

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitCode(this);

    public override CodeFragment Clone() => new()
    {
        Content = Content.Clone()
    };
}