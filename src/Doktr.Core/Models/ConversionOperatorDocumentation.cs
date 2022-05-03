using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public enum ConversionKind
{
    Implicit,
    Explicit
}

public class ConversionOperatorDocumentation : MemberDocumentation,
    IHasStatic,
    IHasParameters,
    IHasReturns,
    IHasExceptions
{
    public ConversionOperatorDocumentation(
        string name,
        MemberVisibility visibility,
        TypeSignature resultingType,
        ConversionKind kind)
        : base(name, visibility)
    {
        ReturnType = resultingType;
        Kind = kind;
    }

    public ConversionKind Kind { get; set; }
    public bool IsStatic => true;
    public TypeSignature InputType => Parameters[0].Type;
    public TypeSignature ResultType => ReturnType;
    public ParameterDocumentationCollection Parameters { get; set; } = new();
    public TypeSignature ReturnType { get; set; }
    public DocumentationFragmentCollection Returns { get; set; } = new();
    public ExceptionSegmentCollection Exceptions { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitConversionOperator(this);

    public override ConversionOperatorDocumentation Clone()
    {
        var clone = new ConversionOperatorDocumentation(Name, Visibility, ReturnType.Clone(), Kind);
        CopyDocumentationTo(clone);
        
        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is ConversionOperatorDocumentation otherConvOp)
        {
            otherConvOp.Kind = Kind;
            otherConvOp.Parameters = Parameters.Clone();
            otherConvOp.Returns = Returns.Clone();
            otherConvOp.Exceptions = Exceptions.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}