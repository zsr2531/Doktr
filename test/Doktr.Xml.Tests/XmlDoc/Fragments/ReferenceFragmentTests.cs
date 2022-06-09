using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class ReferenceFragmentTests : FragmentTests
{
    public ReferenceFragmentTests()
        : base(new ReferenceFragmentParser())
    {
    }

    [Fact]
    public void Code_Reference()
    {
        var entry = GetSummaryFor("<see cref='T:Test'/>");

        var reference = AssertSingleChildIsType<CodeReferenceFragment>(entry);
        reference.CodeReference.Identifier.Should().Be("T:Test");
        reference.Replacement.Should().BeNull();
    }

    [Fact]
    public void Link_Reference()
    {
        var entry = GetSummaryFor("<see href='https://example.com'/>");

        var reference = AssertSingleChildIsType<LinkReferenceFragment>(entry);
        reference.Url.Should().Be("https://example.com");
        reference.Replacement.Should().BeNull();
    }

    [Fact]
    public void Code_Reference_With_Replacement()
    {
        var entry = GetSummaryFor("<see cref='T:Test'>Replacement</see>");

        var reference = AssertSingleChildIsType<CodeReferenceFragment>(entry);
        reference.CodeReference.Identifier.Should().Be("T:Test");
        reference.Replacement.Should().NotBeNull();
        var text = AssertSingleChildIsType<TextFragment>(reference.Replacement!);
        text.Text.Should().Be("Replacement");
    }

    [Fact]
    public void Link_Reference_With_Replacement()
    {
        var entry = GetSummaryFor("<see href='https://example.com'>Replacement</see>");

        var reference = AssertSingleChildIsType<LinkReferenceFragment>(entry);
        reference.Url.Should().Be("https://example.com");
        reference.Replacement.Should().NotBeNull();
        var text = AssertSingleChildIsType<TextFragment>(reference.Replacement!);
        text.Text.Should().Be("Replacement");
    }

    [Fact]
    public void Missing_Cref_And_Href()
    {
        var parser = CreateParser("<see/>");
        var result = parser.ParseXmlDoc();

        Assert.NotEmpty(result);
        Assert.True(parser.HasErrors);
    }
}