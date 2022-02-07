namespace Doktr.SemanticDocumentationValidatorTests.TestCases;

#nullable enable

public struct Struct<T>
{
}

public delegate object Delegate(object? a, (dynamic, (int, int), string? Heyy) b, int c);
// public delegate object Delegate(Struct<int>? p);
