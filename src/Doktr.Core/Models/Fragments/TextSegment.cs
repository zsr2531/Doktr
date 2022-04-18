namespace Doktr.Core.Models.Fragments;

public class TextSegment : IDocumentationFragment
{
    public TextSegment(string text)
    {
        Text = text;
    }

    public string Text { get; set; }

    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitText(this);
}