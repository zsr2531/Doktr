namespace Doktr.Lifters.Common.XmlDoc;

public readonly struct TextSpan : IEquatable<TextSpan>
{
    public TextSpan(int startLine, int startColumn, int endLine, int endColumn)
    {
        StartLine = startLine;
        StartColumn = startColumn;
        EndLine = endLine;
        EndColumn = endColumn;
    }

    public int StartLine { get; }
    public int StartColumn { get; }
    public int EndLine { get; }
    public int EndColumn { get; }

    public bool Equals(TextSpan other) => StartLine == other.StartLine && StartColumn == other.StartColumn &&
        EndLine == other.EndLine && EndColumn == other.EndColumn;

    public override bool Equals(object? obj) => obj is TextSpan other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(StartLine, StartColumn, EndLine, EndColumn);

    public override string ToString() => $"{StartLine}:{StartColumn}-{EndLine}:{EndColumn}";

    public static bool operator ==(TextSpan left, TextSpan right) => left.Equals(right);

    public static bool operator !=(TextSpan left, TextSpan right) => !left.Equals(right);
}