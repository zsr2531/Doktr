using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class ItalicFragmentTests : FragmentTests
{
    public ItalicFragmentTests()
        : base(new ItalicFragmentParser())
    {
    }

    [Fact]
    public void Italic()
    {
        var entry = ParseXmlDoc("<i>Hello</i>");

        var italic = AssertSingleChildIsType<ItalicFragment>(entry);
        var text = AssertSingleChildIsType<TextFragment>(italic.Children);
        text.Text.Should().Be("Hello");
    }

    [Fact]
    public void Nested_Italic()
    {
        var entry = ParseXmlDoc("<i><i>Hello</i></i>");

        var italic1 = AssertSingleChildIsType<ItalicFragment>(entry);
        var italic2 = AssertSingleChildIsType<ItalicFragment>(italic1.Children);
        var text = AssertSingleChildIsType<TextFragment>(italic2.Children);
        text.Text.Should().Be("Hello");
    }
}