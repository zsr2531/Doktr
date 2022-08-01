using System;

namespace Doktr.Lifters.AsmResolver.Tests.Utils.TestCases;

public class CodeReferenceTranslatorCases
{
    public event EventHandler Event;

    public string Field;

    public CodeReferenceTranslatorCases(int param)
    {
        Event += (_, _) => { };
        Field = param.ToString();
    }


    public int Property { get; set; }

    public void Method(string param)
    {
    }

    public void MethodWithTwoParams(int first, int second)
    {
    }

    public class GenericInnerClass<T>
    {
        public class GenericInnerInnerClass<U>
        {
        }
    }
}