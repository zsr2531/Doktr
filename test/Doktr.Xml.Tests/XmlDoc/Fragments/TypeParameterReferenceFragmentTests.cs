using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class TypeParameterReferenceFragmentTests : FragmentTests
{
    public TypeParameterReferenceFragmentTests()
        : base(new TypeParameterReferenceFragmentParser())
    {
    }

    [Fact]
    public void TypeParameterReference()
    {
        var entry = GetSummaryFor("<typeparamref name='test'/>");

        var typeparamref = AssertSingleChildIsType<TypeParameterReferenceFragment>(entry);
        typeparamref.Name.Should().Be("test");
    }

    [Fact]
    public void Missing_Name()
    {
        Assert.Throws<XmlDocParserException>(() => GetSummaryFor("<typeparamref/>"));
    }
}