namespace Doktr.Core.Models.Fragments;

public class TextFragment : IDocumentationFragment
{
    public TextFragment(string text)
    {
        Text = text;
    }

    public string Text { get; set; }

    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitText(this);
}