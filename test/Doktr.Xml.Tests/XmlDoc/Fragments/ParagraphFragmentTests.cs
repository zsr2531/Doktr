using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class ParagraphFragmentTests : FragmentTests
{
    public ParagraphFragmentTests()
        : base(new ParagraphFragmentParser())
    {
    }

    [Fact]
    public void Paragraph()
    {
        var entry = ParseXmlDoc("<p>Hello</p>");

        var paragraph = AssertSingleChildIsType<ParagraphFragment>(entry);
        var text = AssertSingleChildIsType<TextFragment>(paragraph.Children);
        text.Text.Should().Be("Hello");
    }

    [Fact]
    public void Nested_Paragraph()
    {
        var entry = ParseXmlDoc("<p><p>Hello</p></p>");

        var paragraph1 = AssertSingleChildIsType<ParagraphFragment>(entry);
        var paragraph2 = AssertSingleChildIsType<ParagraphFragment>(paragraph1.Children);
        var text = AssertSingleChildIsType<TextFragment>(paragraph2.Children);
        text.Text.Should().Be("Hello");
    }
}