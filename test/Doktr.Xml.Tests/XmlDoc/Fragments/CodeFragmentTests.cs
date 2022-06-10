using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class CodeFragmentTests : FragmentTests
{
    public CodeFragmentTests()
        : base(new CodeFragmentParser())
    {
    }

    [Fact]
    public void Code()
    {
        var entry = ParseXmlDoc("<code>\npublic void Test()\n</code>");

        var code = AssertSingleChildIsType<CodeFragment>(entry);
        var text = AssertSingleChildIsType<TextFragment>(code.Content);
        text.Text.Should().Be("\npublic void Test()\n");
    }
}