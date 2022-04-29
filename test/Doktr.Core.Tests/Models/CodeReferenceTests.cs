using System;
using Doktr.Core.Models;
using Xunit;

// ReSharper disable CoVariantArrayConversion

namespace Doktr.Core.Tests.Models;

public class CodeReferenceTests
{
    public static readonly object[][] ValidPrefixes = 
    {
        new object[] { '!' },
        new object[] { 'T' },
        new object[] { 'M' },
        new object[] { 'P' },
        new object[] { 'F' },
        new object[] { 'E' },
        new object[] { 'N' }
    };
    
    public static readonly object[][] ValidPrefixesWithEnum = 
    {
        new object[] { '!', CodeReferenceKind.Error },
        new object[] { 'T', CodeReferenceKind.Type },
        new object[] { 'M', CodeReferenceKind.Method },
        new object[] { 'P', CodeReferenceKind.Property },
        new object[] { 'F', CodeReferenceKind.Field },
        new object[] { 'E', CodeReferenceKind.Event },
        new object[] { 'N', CodeReferenceKind.Namespace }
    };
    
    [Fact]
    public void Empty_String_Should_Throw_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new CodeReference(string.Empty));
    }
    
    [Theory]
    [MemberData(nameof(ValidPrefixesWithEnum))]
    public void Valid_Prefix_Colon_And_Data_Should_Construct(char prefix, CodeReferenceKind kind)
    {
        var codeReference = new CodeReference(prefix + ":test");
        Assert.Equal("test", codeReference.Name.ToString());
        Assert.Equal(prefix, codeReference.Prefix);
        Assert.Equal(kind, codeReference.Kind);
    }
    
    [Fact]
    public void No_Data_After_Colon_Should_Throw_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new CodeReference("T:"));
    }
    
    [Theory]
    [MemberData(nameof(ValidPrefixes))]
    public void Only_Valid_Prefix_Should_Throw_ArgumentException(char prefix)
    {
        Assert.Throws<ArgumentException>(() => new CodeReference($"{prefix}"));
    }
    
    [Fact]
    public void Two_Or_More_Dots_After_Colon_Should_Throw_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new CodeReference("T:test..test"));
    }
    
    [Fact]
    public void No_Colon_At_Second_Position_Should_Throw_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new CodeReference("Ttest"));
    }
    
    [Fact]
    public void Invalid_Prefix_Should_Throw_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new CodeReference("X:test"));
    }
}