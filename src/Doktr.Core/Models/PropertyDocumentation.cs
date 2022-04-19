using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class PropertyDocumentation : MemberDocumentation, IHasStatic, IHasVirtual, IHasValue, IHasExceptions
{
    public PropertyDocumentation(string name, MemberVisibility visibility, TypeSignature type)
        : base(name, visibility)
    {
        Type = type;
    }
    
    public TypeSignature Type { get; set; }
    public bool IsStatic { get; set; } = false;
    public bool IsVirtual { get; set; } = false;
    public bool IsOverride { get; set; } = false;
    public bool IsAbstract { get; set; } = false;
    public bool IsSealed { get; set; } = false;
    public PropertyGetter? Getter { get; set; }
    public PropertySetter? Setter { get; set; }
    public DocumentationFragmentCollection Value { get; set; } = new();
    public ExceptionSegmentCollection Exceptions { get; set; } = new();

    public override PropertyDocumentation Clone()
    {
        var clone = new PropertyDocumentation(Name, Visibility, Type.Clone());
        CopyDocumentationTo(clone);
        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is not PropertyDocumentation otherProperty)
            throw new InvalidOperationException("Cannot copy documentation to a non-property member.");

        otherProperty.IsStatic = IsStatic;
        otherProperty.IsVirtual = IsVirtual;
        otherProperty.IsOverride = IsOverride;
        otherProperty.IsAbstract = IsAbstract;
        otherProperty.IsSealed = IsSealed;
        otherProperty.Getter = Getter?.Clone();
        otherProperty.Setter = Setter?.Clone();
        otherProperty.Value = Value.Clone();
        otherProperty.Exceptions = Exceptions.Clone();
        base.CopyDocumentationTo(other);
    }

    public class PropertyGetter : ICloneable
    {
        public MemberVisibility Visibility { get; set; } = MemberVisibility.Public;

        public PropertyGetter Clone() => new()
        {
            Visibility = Visibility
        };

        object ICloneable.Clone() => Clone();
    }
    
    public class PropertySetter : ICloneable
    {
        public MemberVisibility Visibility { get; set; } = MemberVisibility.Public;
        public bool IsInit { get; set; } = false;

        public PropertySetter Clone() => new()
        {
            Visibility = Visibility,
            IsInit = IsInit
        };

        object ICloneable.Clone() => Clone();
    }
}