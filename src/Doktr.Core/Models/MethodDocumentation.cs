using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public class MethodDocumentation : MemberDocumentation,
    IHasStatic,
    IHasReadOnly,
    IHasVirtual,
    IHasTypeParameters,
    IHasParameters,
    IHasExceptions
{
    public MethodDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public bool IsStatic { get; set; } = false;
    public bool IsReadOnly { get; set; } = false;
    public bool IsVirtual { get; set; } = false;
    public bool IsOverride { get; set; } = false;
    public bool IsAbstract { get; set; } = false;
    public bool IsSealed { get; set; } = false;
    public bool IsExtension { get; set; } = false;
    public TypeParameterSegmentCollection TypeParameters { get; set; } = new();
    public ParameterSegmentCollection Parameters { get; set; } = new();
    public DocumentationFragmentCollection Returns { get; set; } = new();
    public ExceptionSegmentCollection Exceptions { get; set; } = new();
}