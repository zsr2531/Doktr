using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class MemberDocumentation : IEquatable<MemberDocumentation>, ICloneable
{
    protected MemberDocumentation(string name, MemberVisibility visibility)
    {
        Name = name;
        Visibility = visibility;
    }

    public string Name { get; set; }
    public MemberVisibility Visibility { get; set; }
    public DocumentationFragmentCollection Summary { get; set; } = new();
    public DocumentationFragmentCollection Examples { get; set; } = new();
    public DocumentationFragmentCollection Remarks { get; set; } = new();
    public ProductVersionsSegmentCollection AppliesTo { get; set; } = new();

    public abstract MemberDocumentation Clone();

    object ICloneable.Clone() => Clone();
    
    protected virtual void CopyDocumentationTo(MemberDocumentation other)
    {
        other.Summary = Summary.Clone();
        other.Examples = Examples.Clone();
        other.Remarks = Remarks.Clone();
        other.AppliesTo = AppliesTo.Clone();
    }

    public bool Equals(MemberDocumentation? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;

        return Equals((MemberDocumentation) obj);
    }

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(MemberDocumentation? left, MemberDocumentation? right) => Equals(left, right);

    public static bool operator !=(MemberDocumentation? left, MemberDocumentation? right) => !Equals(left, right);
}