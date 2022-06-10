using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class MonospaceFragmentTests : FragmentTests
{
    public MonospaceFragmentTests()
        : base(new MonospaceFragmentParser())
    {
    }

    [Fact]
    public void Monospace()
    {
        var entry = ParseXmlDoc("<c>Hello</c>");

        var monospace = AssertSingleChildIsType<MonospaceFragment>(entry);
        var text = AssertSingleChildIsType<TextFragment>(monospace.Children);

        text.Text.Should().Be("Hello");
    }

    [Fact]
    public void Nested_Bold()
    {
        var entry = ParseXmlDoc("<c><c>Hello</c></c>");

        var monospace = AssertSingleChildIsType<MonospaceFragment>(entry);
        var monospace2 = AssertSingleChildIsType<MonospaceFragment>(monospace.Children);
        var text = AssertSingleChildIsType<TextFragment>(monospace2.Children);
        text.Text.Should().Be("Hello");
    }
}