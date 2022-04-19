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
        TypeSignature returnType,
        ConversionKind kind)
        : base(name, visibility)
    {
        ReturnType = returnType;
        Kind = kind;
    }

    public ConversionKind Kind { get; set; }
    public bool IsStatic => true;
    public ParameterSegmentCollection Parameters { get; set; } = new();
    public TypeSignature ReturnType { get; set; }
    public DocumentationFragmentCollection Returns { get; set; } = new();
    public ExceptionSegmentCollection Exceptions { get; set; } = new();

    public override ConversionOperatorDocumentation Clone()
    {
        var clone = new ConversionOperatorDocumentation(Name, Visibility, ReturnType.Clone(), Kind)
        {
            Parameters = Parameters.Clone(),
            Returns = Returns.Clone(),
            Exceptions = Exceptions.Clone()
        };

        CopyDocumentationTo(clone);
        return clone;
    }
}