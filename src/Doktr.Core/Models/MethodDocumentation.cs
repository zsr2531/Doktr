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
    public bool IsOverride { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public bool IsExtension { get; set; }
    public TypeParameterSegmentCollection TypeParameters { get; set; } = new();
    public ParameterSegmentCollection Parameters { get; set; } = new();
    public TypeSignature ReturnType { get; set; }
    public DocumentationFragmentCollection Returns { get; set; } = new();
    public ExceptionSegmentCollection Exceptions { get; set; } = new();

    public override MethodDocumentation Clone()
    {
        var clone = new MethodDocumentation(Name, Visibility, ReturnType)
        {
            IsStatic = IsStatic,
            IsReadOnly = IsReadOnly,
            IsVirtual = IsVirtual,
            IsOverride = IsOverride,
            IsAbstract = IsAbstract,
            IsSealed = IsSealed,
            IsExtension = IsExtension,
            TypeParameters = TypeParameters.Clone(),
            Parameters = Parameters.Clone(),
            Returns = Returns.Clone(),
            Exceptions = Exceptions.Clone()
        };
        
        CopyDocumentationTo(clone);
        return clone;
    }
}