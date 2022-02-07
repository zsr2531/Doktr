using System;
using System.Collections.Generic;
using System.Linq;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using Doktr.Services.Attributes;

namespace Doktr.Services;

public class NullabilityAwareTypeSignatureVisitor : ITypeSignatureVisitor<string>
{
    public delegate (string Name, bool IsValueType) GetGenericParameterInfo(GenericParameterType type, int index);

    private static readonly Dictionary<string, string> BuiltIns = new()
    {
        ["Object"] = "object",
        ["String"] = "string",
        ["Int8"] = "sbyte",
        ["Int16"] = "short",
        ["Int32"] = "int",
        ["Int64"] = "long",
        ["UInt8"] = "byte",
        ["UInt16"] = "ushort",
        ["UInt32"] = "uint",
        ["UInt64"] = "ulong",
        ["Single"] = "float",
        ["Double"] = "double"
    };

    private readonly IAttributeDataProvider<Nullability> _nullabilityProvider;
    private readonly IAttributeDataProvider<bool> _dynamicValues;
    private readonly IAttributeDataProvider<string?> _tupleElementNames;
    private readonly IAttributeDataProvider<bool> _nativeIntegerValues;
    private readonly GetGenericParameterInfo _genericInfoFunc;
    private readonly bool _dropRef;

    public NullabilityAwareTypeSignatureVisitor(
        IAttributeDataProvider<Nullability> nullabilityProvider,
        IAttributeDataProvider<bool> dynamicValues,
        IAttributeDataProvider<string?> tupleElementNames,
        IAttributeDataProvider<bool> nativeIntegerValues,
        GetGenericParameterInfo genericInfoFunc,
        bool dropRef = false)
    {
        _nullabilityProvider = nullabilityProvider;
        _dynamicValues = dynamicValues;
        _tupleElementNames = tupleElementNames;
        _nativeIntegerValues = nativeIntegerValues;
        _genericInfoFunc = genericInfoFunc;
        _dropRef = dropRef;
    }

    public string VisitArrayType(ArrayTypeSignature signature)
    {
        bool nullable = NextNullable();
        _dynamicValues.Next();
        string inner = signature.BaseType.AcceptVisitor(this);

        return $"{inner}[{new string(',', signature.Dimensions.Count - 1)}]{(nullable ? "?" : "")}";
    }

    public string VisitBoxedType(BoxedTypeSignature signature)
    {
        throw new NotSupportedException();
    }

    public string VisitByReferenceType(ByReferenceTypeSignature signature)
    {
        string inner = signature.BaseType.AcceptVisitor(this);

        return _dropRef
            ? inner
            : $"ref {inner}";
    }

    public string VisitCorLibType(CorLibTypeSignature signature)
    {
        bool isNullable = !signature.IsValueType && NextNullable();
        bool isDynamic = _dynamicValues.Next();
        if (isDynamic)
        {
            return isNullable
                ? "dynamic?"
                : "dynamic";
        }

        string name = BuiltIns.TryGetValue(signature.Name, out string? builtin)
            ? builtin
            : signature.Name;

        if (name is "IntPtr" or "UIntPtr" && _nativeIntegerValues.Next())
        {
            name = name == "IntPtr"
                ? "nint"
                : "nuint";
        }
        
        return isNullable
            ? name + '?'
            : name;
    }

    public string VisitCustomModifierType(CustomModifierTypeSignature signature)
    {
        throw new NotSupportedException();
    }

    public string VisitGenericInstanceType(GenericInstanceTypeSignature signature)
    {
        _dynamicValues.Next();
        var type = signature.GenericType;
        
        if (type.Namespace == "System" && type.Name.StartsWith("ValueTuple"))
        {
            _nullabilityProvider.Next();
            var names = Enumerable.Repeat(0, signature.TypeArguments.Count).Select(_ => _tupleElementNames.Next()).ToList();
            var elements = signature.TypeArguments.Select((arg, idx) =>
            {
                // ReSharper disable once VariableHidesOuterVariable
                string type = arg.AcceptVisitor(this);
                string? name = names[idx];
                
                return name is null
                    ? type
                    : $"{type} {name}";
            });

            return $"({string.Join(", ", elements)})";
        }
        
        if (type.IsTypeOf("System", "Nullable`1"))
        {
            string arg = signature.TypeArguments[0].AcceptVisitor(this);
            return arg + '?';
        }
        
        bool isNullable = NextNullable();
        var args = signature.TypeArguments.Select(arg => arg.AcceptVisitor(this));
        string name = DropBackticks(signature.GenericType.Name);

        return $"{name}<{string.Join(", ", args)}>{(isNullable ? "?" : "")}";
    }

    public string VisitGenericParameter(GenericParameterSignature signature)
    {
        (string name, _) = _genericInfoFunc(signature.ParameterType, signature.Index);
        bool isNullable = NextNullable();
        _dynamicValues.Next();

        return isNullable
            ? name + '?'
            : name;
    }

    public string VisitPinnedType(PinnedTypeSignature signature)
    {
        throw new NotSupportedException();
    }

    public string VisitPointerType(PointerTypeSignature signature)
    {
        _nullabilityProvider.Next();
        _dynamicValues.Next();
        string inner = signature.BaseType.AcceptVisitor(this);

        return inner + '*';
    }

    public string VisitSentinelType(SentinelTypeSignature signature)
    {
        throw new NotSupportedException();
    }

    public string VisitSzArrayType(SzArrayTypeSignature signature)
    {
        bool isNullable = NextNullable();
        _dynamicValues.Next();
        string inner = signature.BaseType.AcceptVisitor(this);

        return $"{inner}[]{(isNullable ? "?" : "")}";
    }

    public string VisitTypeDefOrRef(TypeDefOrRefSignature signature)
    {
        string name = DropBackticks(signature.Type.Name);
        bool isNullable = !signature.IsValueType && NextNullable();
        _dynamicValues.Next();

        return isNullable
            ? name + '?'
            : name;
    }

    public string VisitFunctionPointerType(FunctionPointerTypeSignature signature)
    {
        var methodSignature = signature.Signature;
        _nullabilityProvider.Next();
        _dynamicValues.Next();
        string ret = methodSignature.ReturnType.AcceptVisitor(this);
        string parameters = string.Join(", ", methodSignature.ParameterTypes.Select(p => p.AcceptVisitor(this)));

        var conv = methodSignature.Attributes;
        if ((conv & CallingConventionAttributes.C) != 0)
            return $"delegate* unmanaged[Cdecl]<{parameters}, {ret}>";
        if ((conv & CallingConventionAttributes.FastCall) != 0)
            return $"delegate* unmanaged[Fastcall]<{parameters}, {ret}>";
        if ((conv & CallingConventionAttributes.StdCall) != 0)
            return $"delegate* unmanaged[Stdcall]<{parameters}, {ret}>";
        if ((conv & CallingConventionAttributes.ThisCall) != 0)
            return $"delegate* unmanaged[Thiscall]<{parameters}, {ret}>";

        return $"delegate*<{parameters}, {ret}>";
    }

    private bool NextNullable()
    {
        return _nullabilityProvider.Next() == Nullability.Nullable;
    }

    private static string DropBackticks(string name)
    {
        int index = name.IndexOf('`');
        return index == -1
            ? name
            : name[..index];
    }

    public class ValueTypeCheckerVisitor : ITypeSignatureVisitor<bool>
    {
        private readonly GetGenericParameterInfo _genericInfoFunc;

        public ValueTypeCheckerVisitor(GetGenericParameterInfo genericInfoFunc)
        {
            _genericInfoFunc = genericInfoFunc;
        }

        public bool VisitArrayType(ArrayTypeSignature signature) => false;

        public bool VisitBoxedType(BoxedTypeSignature signature) => throw new NotSupportedException();

        public bool VisitByReferenceType(ByReferenceTypeSignature signature) => false;

        public bool VisitCorLibType(CorLibTypeSignature signature) => signature.Type.IsValueType;

        public bool VisitCustomModifierType(CustomModifierTypeSignature signature) => throw new NotSupportedException();

        public bool VisitGenericInstanceType(GenericInstanceTypeSignature signature)
        {
            return signature.GenericType.IsValueType && signature.TypeArguments.All(arg => arg.AcceptVisitor(this));
        }

        public bool VisitGenericParameter(GenericParameterSignature signature)
        {
            (_, bool isValueType) = _genericInfoFunc(signature.ParameterType, signature.Index);
            return isValueType;
        }

        public bool VisitPinnedType(PinnedTypeSignature signature) => throw new NotSupportedException();

        public bool VisitPointerType(PointerTypeSignature signature) => true;

        public bool VisitSentinelType(SentinelTypeSignature signature) => throw new NotSupportedException();

        public bool VisitSzArrayType(SzArrayTypeSignature signature) => false;

        public bool VisitTypeDefOrRef(TypeDefOrRefSignature signature) => signature.Type.IsValueType;

        public bool VisitFunctionPointerType(FunctionPointerTypeSignature signature) => true;
    }
}