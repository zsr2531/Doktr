using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class ParagraphFragment : DocumentationFragment
{
    public DocumentationFragmentCollection Children { get; set; } = new();
    
    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitParagraph(this);

    public override ParagraphFragment Clone() => new()
    {
        Children = Children.Clone()
    };
}