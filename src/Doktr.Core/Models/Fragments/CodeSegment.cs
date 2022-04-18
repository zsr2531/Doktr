namespace Doktr.Core.Models.Fragments;

public class CodeSegment : IDocumentationFragment
{
    public CodeSegment(string code)
    {
        Code = code;
    }

    public string Code { get; set; }

    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitCode(this);
}