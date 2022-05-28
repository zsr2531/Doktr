using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class BoldFragmentTests : FragmentTests
{
    public BoldFragmentTests()
        : base(new BoldFragmentParser())
    {
    }

    [Fact]
    public void Bold()
    {
        var entry = GetSummaryFor("<b>Hello</b>");

        var bold = AssertSingleChildIsType<BoldFragment>(entry);
        var text = AssertSingleChildIsType<TextFragment>(bold.Children);
        text.Text.Should().Be("Hello");
    }

    [Fact]
    public void Nested_Bold()
    {
        var entry = GetSummaryFor("<b><b>Hello</b></b>");

        var bold1 = AssertSingleChildIsType<BoldFragment>(entry);
        var bold2 = AssertSingleChildIsType<BoldFragment>(bold1.Children);
        var text = AssertSingleChildIsType<TextFragment>(bold2.Children);
        text.Text.Should().Be("Hello");
    }
}