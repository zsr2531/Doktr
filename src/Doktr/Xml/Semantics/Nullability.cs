using System;

namespace Doktr.Xml.Semantics
{
    public enum Nullability : byte
    {
        Oblivious = 0,
        NotAnnotated = 1,
        Annotated = 2
    }

    public static class NullabilityExtensions
    {
        public static string ToString(this Nullability nullability) => nullability switch
        {
            Nullability.NotAnnotated => "",
            Nullability.Annotated => "?",
            _ => throw new ArgumentOutOfRangeException(nameof(nullability))
        };
    }
}