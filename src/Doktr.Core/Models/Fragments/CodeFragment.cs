namespace Doktr.Core.Models.Fragments;

public class CodeFragment : IDocumentationFragment
{
    public CodeFragment(string code)
    {
        Code = code;
    }

    public string Code { get; set; }

    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitCode(this);
}