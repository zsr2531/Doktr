namespace Doktr.Core.Models;

public readonly struct CodeReference : IEquatable<CodeReference>
{
    private const char ErrorPrefix = '!';
    private const char TypePrefix = 'T';
    private const char MethodPrefix = 'M';
    private const char PropertyPrefix = 'P';
    private const char FieldPrefix = 'F';
    private const char EventPrefix = 'E';
    private const char NamespacePrefix = 'N';
    private const char Colon = ':';

    public CodeReference(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
            throw new ArgumentException("Identifier must be non-null and non-empty.", nameof(identifier));
        if (identifier.Length < 3)
            throw new ArgumentException("Identifier must contain at least 3 characters.", nameof(identifier));
        if (identifier[1] != Colon)
            throw new ArgumentException("Identifier must have a colon after the prefix.", nameof(identifier));
        if (identifier.Contains(".."))
            throw new ArgumentException("Identifier cannot contain two or more dots in a row.", nameof(identifier));
        

        Identifier = identifier;
        Kind = identifier[0] switch
        {
            ErrorPrefix => CodeReferenceKind.Error,
            TypePrefix => CodeReferenceKind.Type,
            MethodPrefix => CodeReferenceKind.Method,
            PropertyPrefix => CodeReferenceKind.Property,
            FieldPrefix => CodeReferenceKind.Field,
            EventPrefix => CodeReferenceKind.Event,
            NamespacePrefix => CodeReferenceKind.Namespace,
            _ => throw new ArgumentException("Identifier must start with a valid prefix.", nameof(identifier))
        };
    }

    public string Identifier { get; }
    public CodeReferenceKind Kind { get; }
    public ReadOnlySpan<char> Name => Identifier[2..];
    public char Prefix => Identifier[0];
    public bool IsError => Kind == CodeReferenceKind.Error;
    public bool IsType => Kind == CodeReferenceKind.Type;
    public bool IsMethod => Kind == CodeReferenceKind.Method;
    public bool IsProperty => Kind == CodeReferenceKind.Property;
    public bool IsField => Kind == CodeReferenceKind.Field;
    public bool IsEvent => Kind == CodeReferenceKind.Event;
    public bool IsNamespace => Kind == CodeReferenceKind.Namespace;

    public bool Equals(CodeReference other) => Identifier == other.Identifier;

    public override bool Equals(object? obj) => obj is CodeReference other && Equals(other);

    public override int GetHashCode() => Identifier.GetHashCode();

    public static bool operator ==(CodeReference left, CodeReference right) => left.Equals(right);

    public static bool operator !=(CodeReference left, CodeReference right) => !left.Equals(right);
}