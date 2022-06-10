namespace Doktr.Xml;

public readonly struct TextSpan : IEquatable<TextSpan>
{
    public TextSpan(int line, int column)
    {
        StartLine = line;
        StartColumn = column;
        EndLine = line;
        EndColumn = column + 1;
    }

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

    // This does not do any sanity checks! ie. if the two spans are disjunct
    public TextSpan CombineWith(TextSpan end)
    {
        int startLine = Math.Min(StartLine, end.StartLine);
        int startColumn = StartLine == end.StartLine
            ? Math.Min(StartColumn, end.StartColumn)
            : startLine == StartLine
                ? StartColumn
                : end.StartColumn;
        int endLine = Math.Max(EndLine, end.EndLine);
        int endColumn  = EndLine == end.EndLine
            ? Math.Max(EndColumn, end.EndColumn)
            : endLine == EndLine
                ? EndColumn
                : end.EndColumn;

        return new TextSpan(startLine, startColumn, endLine, endColumn);
    }

    public void Deconstruct(out int startLine, out int startColumn, out int endLine, out int endColumn)
    {
        startLine = StartLine;
        endLine = EndLine;
        startColumn = StartColumn;
        endColumn = EndColumn;
    }

    public bool Equals(TextSpan other) => StartLine == other.StartLine && StartColumn == other.StartColumn &&
        EndLine == other.EndLine && EndColumn == other.EndColumn;

    public override bool Equals(object? obj) => obj is TextSpan other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(StartLine, StartColumn, EndLine, EndColumn);

    public override string ToString() => $"{StartLine}:{StartColumn}-{EndLine}:{EndColumn}";

    public static bool operator ==(TextSpan left, TextSpan right) => left.Equals(right);

    public static bool operator !=(TextSpan left, TextSpan right) => !left.Equals(right);
}