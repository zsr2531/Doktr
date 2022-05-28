using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class LinkReferenceFragment : DocumentationFragment
{
    public LinkReferenceFragment(string url)
    {
        Url = url;
    }

    public string Url { get; set; }
    public DocumentationFragmentCollection? Replacement { get; set; }

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitLinkReference(this);

    public override LinkReferenceFragment Clone() => new(Url)
    {
        Replacement = Replacement?.Clone()
    };
}