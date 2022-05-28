namespace Doktr.Decompiler;

public static class StringUtils
{
    public static ReadOnlySpan<char> TrimUntilLastDot(this ReadOnlySpan<char> str)
    {
        int lastDot = str.LastIndexOf('.') + 1;
        return lastDot == -1
            ? str
            : str[lastDot..];
    }

    public static ReadOnlySpan<char> TrimTicks(this ReadOnlySpan<char> str)
    {
        int tick = str.LastIndexOf('`');
        return tick == -1
            ? str
            : str[..tick];
    }
}