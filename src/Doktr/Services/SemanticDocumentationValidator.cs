using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using Doktr.Dependencies;
using Doktr.Models;
using Doktr.Services.Attributes;
using Serilog;

namespace Doktr.Services;

public class SemanticDocumentationValidator : ISemanticDocumentationValidator
{
    private readonly ILogger _logger;
    private readonly Stack<Nullability> _nullableContext = new();

    public SemanticDocumentationValidator(ILogger logger)
    {
        _logger = logger;
    }

    public ImmutableArray<ImmutableArray<TypeDocumentation>> ValidateDocumentation(
        DependencyGraph dependencyGraph,
        ImmutableDictionary<IFullNameProvider, XmlDocEntry> documentation)
    {
        var roots = ImmutableArray.CreateBuilder<ImmutableArray<TypeDocumentation>>();

        foreach (var root in dependencyGraph.Roots)
        {
            var assembly = (AssemblyDefinition) root.MetadataMember;
            var types = ImmutableArray.CreateBuilder<TypeDocumentation>();

            foreach (var type in root.Children)
            {
                var member = (TypeDefinition) type.MetadataMember;
                if (!member.IsPublic)
                    continue;
                
                ValidateType(type, documentation, types, "");
            }

            roots.Add(types.ToImmutable());
        }

        return roots.ToImmutable();
    }

    private void ValidateType(
        DependencyNode node,
        ImmutableDictionary<IFullNameProvider, XmlDocEntry> documentation,
        ImmutableArray<TypeDocumentation>.Builder builder,
        string parents)
    {
        var type = (TypeDefinition) node.MetadataMember;
        PushNullableContext(type);
        
        string name = GetName();
        string source = GetSignature(type);
        var genericParameters = ImmutableArray.CreateBuilder<GenericParameterDocumentation>();
        var staticEvents = ImmutableArray.CreateBuilder<EventDocumentation>();
        var staticFields = ImmutableArray.CreateBuilder<FieldDocumentation>();
        var staticProperties = ImmutableArray.CreateBuilder<PropertyDocumentation>();
        var staticMethods = ImmutableArray.CreateBuilder<MethodDocumentation>();
        var constructors = ImmutableArray.CreateBuilder<ConstructorDocumentation>();
        var instanceEvents = ImmutableArray.CreateBuilder<EventDocumentation>();
        var instanceFields = ImmutableArray.CreateBuilder<FieldDocumentation>();
        var instanceProperties = ImmutableArray.CreateBuilder<PropertyDocumentation>();
        var instanceMethods = ImmutableArray.CreateBuilder<MethodDocumentation>();
        var operators = ImmutableArray.CreateBuilder<MethodDocumentation>();
        
        foreach (var child in node.Children.Where(c => c.MetadataMember is TypeDefinition))
            ValidateType(child, documentation, builder, name);

        _nullableContext.Pop();

        string GetName()
        {
            string raw = type.Name;
            int index = raw.IndexOf('`');
            string real = index == -1
                ? raw
                : raw[..index];

            return parents.Length == 0
                ? real
                : $"{parents}.{real}";
        }
    }

    private void PushNullableContext(IHasCustomAttribute member)
    {
        var context = GetNullableContext(member);
        var previous = _nullableContext.TryPeek(out var prev)
            ? prev
            : Nullability.Nullable;
        
        _nullableContext.Push(context ?? previous);
    }

    public string GetSignature(TypeDefinition type)
    {
        var builder = new StringBuilder();

        if (type.IsValueType)
            WriteStructSignature(type, builder);
        else if (type.IsInterface)
            WriteInterfaceSignature(type, builder);
        else
            WriteClassSignature(type, builder);

        bool hasProperBase = HasProperBase(type);
        if (hasProperBase)
        {
            builder.Append(" : ");
            builder.Append(TypeSignatureToSource(type.BaseType!.ToTypeSignature()));
        }

        if (type.Interfaces.Count > 0)
            builder.Append(hasProperBase ? ", " : " : ");

        var interfaces = type.Interfaces.Select(impl => TypeSignatureToSource(impl.Interface.ToTypeSignature()));
        builder.Append(string.Join(", ", interfaces));

        WriteGenericParameterConstraints(type, builder);

        return builder.ToString();
    }

    private void WriteClassSignature(TypeDefinition type, StringBuilder builder)
    {
        if (IsDelegate(type))
        {
            WriteDelegateSignature(type, builder);
            return;
        }
        
        if (IsRecord(type))
        {
            WriteRecordSignature(type, builder);
            return;
        }

        string modifiers = GetAccessModifiers(type);
        string secondaryModifiers = GetSecondaryAccessModifiers(type);
        
        builder.Append(modifiers);
        if (!string.IsNullOrEmpty(secondaryModifiers))
        {
            builder.Append(' ');
            builder.Append(secondaryModifiers);
        }

        builder.Append(" class ");
        builder.Append(TypeSignatureToSource(type.ToTypeSignature()));

        WriteGenericParameters(type, builder);

        static bool IsDelegate(TypeDefinition type) => type.BaseType?.IsTypeOf("System", "MulticastDelegate") ?? false;

        static bool IsRecord(TypeDefinition type) => type.Methods.FirstOrDefault(m => m.Name == "<Clone>$") is not null;
    }

    private void WriteDelegateSignature(TypeDefinition type, StringBuilder builder)
    {
        string modifiers = GetAccessModifiers(type);
        builder.Append(modifiers);
        builder.Append(" delegate ");

        var invoke = type.Methods.Single(m => m.Name == "Invoke");
        var signature = invoke.Signature;
        var ret = invoke.ParameterDefinitions[0];
        PushNullableContext(ret);
        // builder.Append(TypeSignatureToSource(signature.ReturnType, GetNullablilityProvider(ret)));
        _nullableContext.Pop();

        builder.Append(' ');
        builder.Append(TypeSignatureToSource(type.ToTypeSignature()));
        WriteGenericParameters(type, builder);
        
        builder.Append('(');
        var parameters = invoke.Parameters.Select(p =>
        {
            var def = p.Definition;
            PushNullableContext(def);
            string modifier = def.IsIn ? "in " : def.IsOut ? "out " : "";
            // var visitor = new NullabilityAwareTypeSignatureVisitor(
            //     GetNullablilityProvider(def),
            //     (_, i) => type.GenericParameters[i].Name,
            //     modifier != "");
            
            // string source = $"{modifier}{p.ParameterType.AcceptVisitor(visitor)} {p.Name}";
            _nullableContext.Pop();

            // return source;
            return "";
        });
        builder.Append(string.Join(", ", parameters));
        builder.Append(')');
    }

    private void WriteRecordSignature(TypeDefinition type, StringBuilder builder)
    {
        string modifiers = GetAccessModifiers(type);
        string secondaryModifiers = GetSecondaryAccessModifiers(type);
        builder.Append(modifiers);

        if (!string.IsNullOrEmpty(secondaryModifiers))
        {
            builder.Append(' ');
            builder.Append(secondaryModifiers);
        }

        builder.Append(" record ");
        builder.Append(TypeSignatureToSource(type.ToTypeSignature()));
        
        WriteGenericParameters(type, builder);

        var deconstruct = type.Methods.Single(m => m.Name == "Deconstruct");
        var parameters = deconstruct.Parameters.Select(p =>
        {
            PushNullableContext(p.Definition);
            // string source = $"{TypeSignatureToSource(p.ParameterType, GetNullablilityProvider(p.Definition))} {p.Name}";
            _nullableContext.Pop();

            // return source;
            return "";
        });
        builder.Append('(');
        builder.Append(string.Join(", ", parameters));
        builder.Append(')');
    }

    private static void WriteInterfaceSignature(TypeDefinition type, StringBuilder builder)
    {
        string modifiers = GetAccessModifiers(type);
        builder.Append(modifiers);

        builder.Append(" interface ");
        builder.Append(TypeSignatureToSource(type.ToTypeSignature()));
        
        WriteGenericParameters(type, builder);
    }

    private static void WriteStructSignature(TypeDefinition type, StringBuilder builder)
    {
        // struct or record struct or enum
        if (IsRecordStruct(type))
        {
            // record struct
        }
        else if (type.IsEnum)
        {
            // enum
        }

        // Not the most robust way, but should work just fine as long as the user
        // doesn't have an identical "PrintMembers" method on a normal struct for whatever reason...
        static bool IsRecordStruct(TypeDefinition type)
        {
            var printMembers = type.Methods.FirstOrDefault(m => m.Name == "PrintMembers");
            if (printMembers is null)
                return false;

            var returnTypeElementType = printMembers.Signature.ReturnType.ElementType;
            if (returnTypeElementType != ElementType.Boolean)
                return false;

            var parameters = printMembers.Parameters;
            return parameters.Count == 2 && parameters[1].ParameterType.IsTypeOf("System", "StringBuilder");
        }
    }

    private static void WriteGenericParameters(TypeDefinition type, StringBuilder builder)
    {
        if (type.GenericParameters.Count <= 0)
            return;
        
        string parameters = string.Join(", ", type.GenericParameters.Select(gp =>
        {
            var attributes = gp.Attributes;
            if ((attributes & GenericParameterAttributes.Covariant) != 0)
                return $"out {gp.Name}";
            if ((attributes & GenericParameterAttributes.Contravariant) != 0)
                return $"in {gp.Name}";
            
            return gp.Name.Value;
        }));
        builder.Append('<');
        builder.Append(parameters);
        builder.Append('>');
    }

    private void WriteGenericParameterConstraints(TypeDefinition type, StringBuilder builder)
    {
        var constraints = new List<string>();
        foreach (var gp in type.GenericParameters)
        {
            var attributes = gp.Attributes;
            var constr = gp.Constraints;
            if ((attributes & GenericParameterAttributes.SpecialConstraintMask) == 0 && constr.Count == 0)
                continue;

            PushNullableContext(gp);
            // var gpNullabilityProvider = GetNullablilityProvider(gp);
            
            var current = new List<string>();
            if ((attributes & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
            {
            }
            // current.Add(gpNullabilityProvider.Next() == Nullability.Nullable ? "class?" : "class");
            else if ((attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
                current.Add("struct");

            foreach (var c in constr)
            {
                PushNullableContext(c);
                // var constraintNullabilityProvider = GetNullablilityProvider(c);
                
                // current.Add(TypeSignatureToSource(c.Constraint.ToTypeSignature(), constraintNullabilityProvider));

                _nullableContext.Pop();
            }

            if ((attributes & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
                current.Add("new()");
            
            constraints.Add(gp.Name + " : " + string.Join(", ", current));

            _nullableContext.Pop();
        }

        if (constraints.Count == 0)
            return;

        builder.Append("\n    where ");
        builder.Append(string.Join("\n    where ", constraints));
    }

    private static string GetAccessModifiers(TypeDefinition type)
    {
        bool isPublic = type.IsPublic;
        bool isNested = type.IsNested;
        if (!isNested)
            return isPublic
                ? "public"
                : "internal";
        
        isPublic = type.IsNestedPublic;
        bool isInternal = type.IsNestedAssembly;
        bool isPrivate = type.IsNestedPrivate;
        bool isProtected = type.IsNestedFamily;
        bool isPrivateProtected = type.IsNestedFamilyAndAssembly;
        bool isProtectedInternal = type.IsNestedFamilyOrAssembly;

        return isPublic
            ? "public"
            : isPrivate
                ? "private"
                : isPrivateProtected
                    ? "private protected"
                    : isProtectedInternal
                        ? "protected internal"
                        : isProtected
                            ? "protected"
                            : isInternal
                                ? "internal"
                                : "";
    }

    private static string GetSecondaryAccessModifiers(TypeDefinition type)
    {
        bool isAbstract = type.IsAbstract;
        bool isSealed = type.IsSealed;
        bool isStatic = isAbstract & isSealed;

        return isStatic
            ? "static"
            : isAbstract
                ? "abstract"
                : isSealed
                    ? "sealed"
                    : "";
    }

    private static string TypeSignatureToSource(TypeSignature type)
    {
        if (type is TypeDefOrRefSignature)
        {
            string source = type.Name;
            if (!source.Contains('`'))
                return source;

            int index = source.IndexOf('`');
            return source[..index];
        }

        var generic = (GenericInstanceTypeSignature) type;
        string args = string.Join(", ", generic.TypeArguments.Select(TypeSignatureToSource));
        string name = TypeSignatureToSource(generic.GenericType.ToTypeSignature());
        return $"{name}<{args}>";
    }

    private static bool HasProperBase(TypeDefinition type) =>
        type.BaseType?.FullName is not null
            and not "System.Enum"
            and not "System.ValueType"
            and not "System.MulticastDelegate"
            and not "System.Object";

    private static Nullability? GetNullableContext(IHasCustomAttribute member)
    {
        var cas = member.CustomAttributes;
        if (cas.Count == 0)
            return null;

        var nullableContext = cas.SingleOrDefault(IsNullableContextAttribute);
        var arg = nullableContext?.Signature.FixedArguments[0];

        return (Nullability?)(byte?) arg?.Element;

        static bool IsNullableContextAttribute(CustomAttribute ca)
        {
            const string ns = "System.Runtime.CompilerServices";
            const string type = "NullableContextAttribute";

            var attributeType = ca.Constructor.DeclaringType;
            return attributeType.IsTypeOf(ns, type);
        }
    }
}