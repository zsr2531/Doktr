using System.Diagnostics.CodeAnalysis;
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
    public bool IsStatic { get; set; }
    public bool IsVirtual { get; set; }
    [MemberNotNullWhen(true, nameof(Overrides))]
    public bool IsOverride { get; set; }
    public CodeReference? Overrides { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public PropertyGetter? Getter { get; set; }
    public PropertySetter? Setter { get; set; }
    public DocumentationFragmentCollection Value { get; set; } = new();
    public ExceptionSegmentCollection Exceptions { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitProperty(this);

    public override PropertyDocumentation Clone()
    {
        var clone = new PropertyDocumentation(Name, Visibility, Type.Clone());
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is PropertyDocumentation otherProperty)
        {
            otherProperty.IsStatic = IsStatic;
            otherProperty.IsVirtual = IsVirtual;
            otherProperty.IsOverride = IsOverride;
            otherProperty.Overrides = Overrides;
            otherProperty.IsAbstract = IsAbstract;
            otherProperty.IsSealed = IsSealed;
            otherProperty.Getter = Getter?.Clone();
            otherProperty.Setter = Setter?.Clone();
            otherProperty.Value = Value.Clone();
            otherProperty.Exceptions = Exceptions.Clone();
        }

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
        public bool IsInit { get; set; }

        public PropertySetter Clone() => new()
        {
            Visibility = Visibility,
            IsInit = IsInit
        };

        object ICloneable.Clone() => Clone();
    }
}