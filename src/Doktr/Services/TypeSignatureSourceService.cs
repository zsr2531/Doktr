using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using Doktr.Services.Attributes;

namespace Doktr.Services;

public class TypeSignatureSourceService : ITypeSignatureSourceService
{
    private static readonly SimpleAttributeDataProvider<string?> NullProvider = new(null);
    private static readonly SimpleAttributeDataProvider<bool> FalseProvider = new(false);
    private static readonly SimpleAttributeDataProvider<bool> TrueProvider = new(true);
    private static readonly SimpleAttributeDataProvider<Nullability> ZeroProvider = new(Nullability.NullOblivious);

    private readonly Stack<Nullability> _nullableAgenda = new();

    public ITypeSignatureSourceService.NullableContext WithContext(IHasCustomAttribute? ca)
    {
        return new ITypeSignatureSourceService.NullableContext(this, ca);
    }

    public void PushNullableContext(IHasCustomAttribute? ca)
    {
        var context = GetNullableContext(ca);
        var previous = _nullableAgenda.TryPeek(out var prev)
            ? prev
            : Nullability.Nullable;

        _nullableAgenda.Push(context ?? previous);
    }

    public void PopNullableContext() => _nullableAgenda.Pop();

    public string GetSource(
        IHasCustomAttribute? member,
        TypeSignature signature,
        NullabilityAwareTypeSignatureVisitor.GetGenericParameterInfo gpInfo,
        bool dropRef = false)
    {
        var nullabilityProvider = GetNullabilityProvider(member);
        var dynamicValues = GetDynamicValues(member);
        var tupleElementNames = GetTupleElementNames(member);
        var nativeIntegerValues = GetNativeIntegerValues(member);
        var valueTypeChecker = new NullabilityAwareTypeSignatureVisitor.ValueTypeCheckerVisitor(gpInfo);
        if (signature.AcceptVisitor(valueTypeChecker))
            nullabilityProvider = ZeroProvider;
        
        var visitor = new NullabilityAwareTypeSignatureVisitor(
            nullabilityProvider,
            dynamicValues,
            tupleElementNames,
            nativeIntegerValues,
            gpInfo,
            dropRef);

        return signature.AcceptVisitor(visitor);
    }

    private IAttributeDataProvider<Nullability> GetNullabilityProvider(IHasCustomAttribute? member)
    {
        if (member is null)
            return new SimpleAttributeDataProvider<Nullability>(_nullableAgenda.Peek());
        
        const string ns = "System.Runtime.CompilerServices";
        const string name = "NullableAttribute";
        var cas = member.CustomAttributes;

        foreach (var ca in cas)
        {
            if (!IsAttributeTypeOf(ca, ns, name, out var arg))
                continue;

            return arg.ArgumentType.IsValueType
                ? new SimpleAttributeDataProvider<Nullability>((Nullability) (byte) arg.Element)
                : new ComplexAttributeDataProvider<Nullability>(arg.Elements.Select(e => (Nullability) (byte) e)
                    .ToArray());
        }

        return new SimpleAttributeDataProvider<Nullability>(_nullableAgenda.Peek());
    }

    private static Nullability? GetNullableContext(IHasCustomAttribute? member)
    {
        if (member is null)
            return null;
        
        const string ns = "System.Runtime.CompilerServices";
        const string name = "NullableContextAttribute";
        var cas = member.CustomAttributes;

        foreach (var ca in cas)
        {
            if (!IsAttributeTypeOf(ca, ns, name, out var arg))
                continue;

            return (Nullability) (byte) arg.Element;
        }

        return null;
    }

    private static IAttributeDataProvider<bool> GetDynamicValues(IHasCustomAttribute? member)
    {
        if (member is null)
            return FalseProvider;
        
        const string ns = "System.Runtime.CompilerServices";
        const string name = "DynamicAttribute";
        var cas = member.CustomAttributes;

        foreach (var ca in cas)
        {
            var ctor = ca.Constructor;
            var type = ctor.DeclaringType;
            if (!type.IsTypeOf(ns, name))
                continue;

            var sig = ca.Signature;
            var args = sig.FixedArguments;
            return args.Count == 0
                ? TrueProvider
                : new ComplexAttributeDataProvider<bool>(args[0].Elements.Select(e => (bool) e).ToArray());
        }

        return FalseProvider;
    }

    private static IAttributeDataProvider<string?> GetTupleElementNames(IHasCustomAttribute? member)
    {
        if (member is null)
            return NullProvider;
        
        const string ns = "System.Runtime.CompilerServices";
        const string name = "TupleElementNamesAttribute";
        var cas = member.CustomAttributes;

        foreach (var ca in cas)
        {
            if (!IsAttributeTypeOf(ca, ns, name, out var arg))
                continue;

            string?[] names = arg.Elements.Select(e => (string?) e).ToArray();
            return new ComplexAttributeDataProvider<string?>(names);
        }

        return NullProvider;
    }

    private static IAttributeDataProvider<bool> GetNativeIntegerValues(IHasCustomAttribute? member)
    {
        if (member is null)
            return FalseProvider;
        
        const string ns = "System.Runtime.CompilerServices";
        const string name = "NativeIntegerAttribute";
        var cas = member.CustomAttributes;

        foreach (var ca in cas)
        {
            var ctor = ca.Constructor;
            var type = ctor.DeclaringType;
            if (!type.IsTypeOf(ns, name))
                continue;

            var sig = ca.Signature;
            var args = sig.FixedArguments;
            return args.Count == 0
                ? TrueProvider
                : new ComplexAttributeDataProvider<bool>(args[0].Elements.Select(e => (bool) e).ToArray());
        }

        return FalseProvider;
    }

    private static bool IsAttributeTypeOf(
        CustomAttribute ca,
        string ns,
        string name,
        [NotNullWhen(true)] out CustomAttributeArgument? arg)
    {
        arg = null;

        var ctor = ca.Constructor;
        var type = ctor.DeclaringType;
        if (!type.IsTypeOf(ns, name))
            return false;

        arg = ca.Signature.FixedArguments[0];
        return true;
    }
}