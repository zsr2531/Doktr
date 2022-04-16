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
    
    private static readonly HashSet<char> ValidPrefixes = new()
    {
        ErrorPrefix,
        TypePrefix,
        MethodPrefix,
        PropertyPrefix,
        FieldPrefix,
        EventPrefix,
        NamespacePrefix
    };

    public CodeReference(string identifier)
    {
        // Identifier must be non-null and non-empty and must contain at least 3 characters.
        // The first character must be a valid prefix.
        // The second character must be a colon.
        // At no point must two or more dots appear in a row.

        if (string.IsNullOrEmpty(identifier))
            throw new ArgumentException("Identifier must be non-null and non-empty.", nameof(identifier));
        if (identifier.Length < 3)
            throw new ArgumentException("Identifier must contain at least 3 characters.", nameof(identifier));
        if (!ValidPrefixes.Contains(identifier[0]))
            throw new ArgumentException("Identifier must start with a valid prefix.", nameof(identifier));
        if (identifier[1] != Colon)
            throw new ArgumentException("Identifier must have a colon after the prefix.", nameof(identifier));
        if (identifier.Contains(".."))
            throw new ArgumentException("Identifier cannot contain two or more dots in a row.", nameof(identifier));
        

        Identifier = identifier;
    }

    public string Identifier { get; }
    public bool IsError => IsFirstChar(ErrorPrefix);
    public bool IsType => IsFirstChar(TypePrefix);
    public bool IsMethod => IsFirstChar(MethodPrefix);
    public bool IsProperty => IsFirstChar(PropertyPrefix);
    public bool IsField => IsFirstChar(FieldPrefix);
    public bool IsEvent => IsFirstChar(EventPrefix);
    public bool IsNamespace => IsFirstChar(NamespacePrefix); // This is unused for .NET, but it's here for completeness.

    public bool Equals(CodeReference other) => Identifier == other.Identifier;

    public override bool Equals(object? obj) => obj is CodeReference other && Equals(other);

    public override int GetHashCode() => Identifier.GetHashCode();
    
    private bool IsFirstChar(char character) => Identifier[0] == character;

    public static bool operator ==(CodeReference left, CodeReference right) => left.Equals(right);

    public static bool operator !=(CodeReference left, CodeReference right) => !left.Equals(right);
}