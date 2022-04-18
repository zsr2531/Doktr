using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class PropertyDocumentation : MemberDocumentation, IHasValue
{
    public PropertyDocumentation(string name, MemberVisibility visibility, TypeSignature type)
        : base(name, visibility)
    {
        Type = type;
    }
    
    public TypeSignature Type { get; set; }
    public PropertyGetter? Getter { get; set; }
    public PropertySetter? Setter { get; set; }
    public DocumentationFragmentCollection Value { get; set; } = new();

    public class PropertyGetter
    {
        public MemberVisibility Visibility { get; set; } = MemberVisibility.Public;
    }
    
    public class PropertySetter
    {
        public MemberVisibility Visibility { get; set; } = MemberVisibility.Public;
        public bool IsInit { get; set; } = false;
    }
}