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
        var clone = new PropertyDocumentation(Name, Visibility, Type)
        {
            IsStatic = IsStatic,
            IsVirtual = IsVirtual,
            IsOverride = IsOverride,
            IsAbstract = IsAbstract,
            IsSealed = IsSealed,
            Getter = Getter?.Clone(),
            Setter = Setter?.Clone(),
            Value = Value.Clone(),
            Exceptions = Exceptions.Clone()
        };
        
        CopyDocumentationTo(clone);
        return clone;
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