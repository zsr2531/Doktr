using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class MethodDocumentation : MemberDocumentation,
    IHasStatic,
    IHasReadOnly,
    IHasVirtual,
    IHasTypeParameters,
    IHasParameters,
    IHasReturns,
    IHasExceptions
{
    public MethodDocumentation(string name, MemberVisibility visibility, TypeSignature returnType)
        : base(name, visibility)
    {
        ReturnType = returnType;
    }

    public bool IsStatic { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsVirtual { get; set; }
    [MemberNotNullWhen(true, nameof(Overrides))]
    public bool IsOverride { get; set; }
    public CodeReference? Overrides { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public bool IsExtension { get; set; }
    public TypeParameterDocumentationCollection TypeParameters { get; set; } = new();
    public ParameterDocumentationCollection Parameters { get; set; } = new();
    public TypeSignature ReturnType { get; set; }
    public DocumentationFragmentCollection Returns { get; set; } = new();
    public ExceptionDocumentationCollection Exceptions { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitMethod(this);

    public override MethodDocumentation Clone()
    {
        var clone = new MethodDocumentation(Name, Visibility, ReturnType);
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is MethodDocumentation otherMethod)
        {
            otherMethod.IsStatic = IsStatic;
            otherMethod.IsReadOnly = IsReadOnly;
            otherMethod.IsVirtual = IsVirtual;
            otherMethod.IsOverride = IsOverride;
            otherMethod.Overrides = Overrides;
            otherMethod.IsAbstract = IsAbstract;
            otherMethod.IsSealed = IsSealed;
            otherMethod.IsExtension = IsExtension;
            otherMethod.TypeParameters = TypeParameters.Clone();
            otherMethod.Parameters = Parameters.Clone();
            otherMethod.Returns = Returns.Clone();
            otherMethod.Exceptions = Exceptions.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}