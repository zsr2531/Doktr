using System.Collections.Generic;

namespace Doktr.Lifters.AsmResolver.Tests.Utils.TestCases;

public unsafe class TypeSignatureTranslatorCases
{
    public string A = null!;
    public int? B;
    public List<Dictionary<int, string>> C = null!;
    public OuterClass<int>.InnerClass<float, double> D = null!;
    public int[] E = null!;
    public int[,,,] F = null!;
    public double* G = null!;
    public delegate*<void> H = null!;

    public void I(ref int a)
    {
    }

    public class OuterClass<T>
    {
        public class InnerClass<U, V>
        {
        }
    }
}