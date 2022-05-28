using Doktr.Core.Models.Constants;

namespace Doktr.Decompiler.Members;

public partial class MemberDecompiler
{
    private static readonly Dictionary<char, string> EscapeSequences = new()
    {
        ['\r'] = "\\r",
        ['\n'] = "\\n",
        ['\t'] = "\\t",
        ['\0'] = "\\0",
        ['\v'] = "\\v",
        ['\f'] = "\\f",
        ['\a'] = "\\a",
        ['\b'] = "\\b",
        ['\"'] = "\\\"",
        ['\\'] = "\\\\",
    };

    public void VisitNull(NullConstant constant) => _sb.Append("null");

    public void VisitString(StringConstant constant) => WriteStringEscaped(constant.Value);

    public void VisitDefault(DefaultConstant constant) => _sb.Append("default");

    public void VisitObject(ObjectConstant constant) => _sb.Append(constant.Value);

    private void WriteStringEscaped(string value)
    {
        _sb.Append('"');

        foreach (char c in value)
        {
            if (EscapeSequences.TryGetValue(c, out string? escaped))
                _sb.Append(escaped);
            else
                _sb.Append(c);
        }

        _sb.Append('"');
    }
}