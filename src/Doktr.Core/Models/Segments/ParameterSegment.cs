using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models.Segments;

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

public class ParameterSegment : ICloneable
{
    public ParameterSegment(TypeSignature type, string name)
    {
        Type = type;
        Name = name;
    }

    public TypeSignature Type { get; set; }
    public string Name { get; set; }
    public ParameterModifierFlags Modifiers { get; set; } = ParameterModifierFlags.None;
    public object? DefaultValue { get; set; }
    public DocumentationFragmentCollection Documentation { get; set; } = new();
    public bool IsIn => (Modifiers & ParameterModifierFlags.In) != 0;
    public bool IsOut => (Modifiers & ParameterModifierFlags.Out) != 0;
    public bool IsRef => (Modifiers & ParameterModifierFlags.Ref) != 0;
    public bool IsOptional => (Modifiers & ParameterModifierFlags.Optional) != 0;
    public bool IsParams => (Modifiers & ParameterModifierFlags.Params) != 0;
    [MemberNotNullWhen(true, nameof(DefaultValue))]
    public bool HasDefaultValue => IsOptional;

    public ParameterSegment Clone() => new(Type.Clone(), Name)
    {
        Modifiers = Modifiers,
        DefaultValue = DefaultValue,
        Documentation = Documentation.Clone()
    };

    object ICloneable.Clone() => Clone();
}