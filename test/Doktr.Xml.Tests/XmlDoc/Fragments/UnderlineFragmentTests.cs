using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class UnderlineFragmentTests : FragmentTests
{
    public UnderlineFragmentTests()
        : base(new UnderlineFragmentParser())
    {
    }

    [Fact]
    public void Underline()
    {
        var entry = GetSummaryFor("<u>Hello</u>");

        var underline = AssertSingleChildIsType<UnderlineFragment>(entry);
        var text = AssertSingleChildIsType<TextFragment>(underline.Children);
        text.Text.Should().Be("Hello");
    }

    [Fact]
    public void Nested_Underline()
    {
        var entry = GetSummaryFor("<u><u>Hello</u></u>");

        var underline1 = AssertSingleChildIsType<UnderlineFragment>(entry);
        var underline2 = AssertSingleChildIsType<UnderlineFragment>(underline1.Children);
        var text = AssertSingleChildIsType<TextFragment>(underline2.Children);
        text.Text.Should().Be("Hello");
    }
}