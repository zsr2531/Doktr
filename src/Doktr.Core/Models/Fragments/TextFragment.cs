namespace Doktr.Core.Models.Fragments;

public class TextFragment : DocumentationFragment
{
    public TextFragment(string text)
    {
        Text = text;
    }

    public string Text { get; set; }

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitText(this);

    public override TextFragment Clone() => new(Text);
}