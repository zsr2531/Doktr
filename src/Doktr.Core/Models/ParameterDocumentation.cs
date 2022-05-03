using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Constants;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

[Flags]
public enum ParameterModifierFlags
{
    None = 0,
    In = 1,
    Out = 2,
    Ref = 4,
    Optional = 8,
    Params = 16
}

public class ParameterDocumentation : ICloneable
{
    public ParameterDocumentation(TypeSignature type, string name)
    {
        Type = type;
        Name = name;
    }

    public TypeSignature Type { get; set; }
    public string Name { get; set; }
    public ParameterModifierFlags Modifiers { get; set; } = ParameterModifierFlags.None;
    public Constant? DefaultValue { get; set; }
    public DocumentationFragmentCollection Documentation { get; set; } = new();
    public bool IsIn => (Modifiers & ParameterModifierFlags.In) != 0;
    public bool IsOut => (Modifiers & ParameterModifierFlags.Out) != 0;
    public bool IsRef => (Modifiers & ParameterModifierFlags.Ref) != 0;
    [MemberNotNullWhen(true, nameof(DefaultValue))]
    public bool IsOptional => (Modifiers & ParameterModifierFlags.Optional) != 0;
    public bool IsParams => (Modifiers & ParameterModifierFlags.Params) != 0;
    [MemberNotNullWhen(true, nameof(DefaultValue))]
    public bool HasDefaultValue => IsOptional;

    public ParameterDocumentation Clone() => new(Type.Clone(), Name)
    {
        Modifiers = Modifiers,
        DefaultValue = DefaultValue,
        Documentation = Documentation.Clone()
    };

    object ICloneable.Clone() => Clone();
}