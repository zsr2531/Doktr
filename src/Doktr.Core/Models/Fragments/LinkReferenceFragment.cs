using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class LinkReferenceFragment : IDocumentationFragment
{
    public LinkReferenceFragment(string url)
    {
        Url = url;
    }

    public string Url { get; set; }
    public DocumentationFragmentCollection? ReplacementText { get; set; }

    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitLinkReference(this);
}