namespace Doktr.Core.Models;

public enum MemberVisibility
{
    Private,
    ProtectedAndAssembly,
    Assembly,
    Protected,
    ProtectedOrAssembly,
    Public
}

public static class MemberVisibilityExtensions
{
    public static bool IsVisibleOutsideAssembly(this MemberVisibility memberVisibility)
    {
        return memberVisibility is MemberVisibility.Public
            or MemberVisibility.Protected
            or MemberVisibility.ProtectedOrAssembly;
    }
}