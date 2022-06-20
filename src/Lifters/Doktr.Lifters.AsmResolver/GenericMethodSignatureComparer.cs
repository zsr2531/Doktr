using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Lifters.AsmResolver;

public class GenericMethodSignatureComparer : EqualityComparer<MethodSignature>
{
    public static readonly GenericMethodSignatureComparer Instance = new();
    private static readonly SignatureComparer Comparer = new();

    private GenericMethodSignatureComparer()
    {
    }

    public bool Equals(MethodSignature x, MethodSignature y, TypeSignature genericProvider)
    {
        var context = CreateGenericContext();
        var xInst = InstantiateMethodSignature(x, context);
        var yInst = InstantiateMethodSignature(y, context);
        return Equals(xInst, yInst);

        GenericContext CreateGenericContext()
        {
            return genericProvider is GenericInstanceTypeSignature generic
                ? new GenericContext(generic, null)
                : default;
        }

        MethodSignature InstantiateMethodSignature(MethodSignature s, GenericContext ctx)
        {
            try
            {
                return s.InstantiateGenericTypes(ctx);
            }
            catch
            {
                return s;
            }
        }
    }

    public override bool Equals(MethodSignature? x, MethodSignature? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x is null || y is null)
            return false;

        return Comparer.Equals(x, y);
    }

    public override int GetHashCode(MethodSignature obj) => Comparer.GetHashCode(obj);
}