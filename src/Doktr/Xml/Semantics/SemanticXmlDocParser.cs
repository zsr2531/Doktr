using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Xml.Semantics
{
    public class SemanticXmlDocParser
    {
        private readonly IDictionary<IFullNameProvider, ImmutableArray<IXmlDocSegment>> _documentation;
        private readonly TypeFormatter _formatter = new();

        public SemanticXmlDocParser(IDictionary<IFullNameProvider, ImmutableArray<IXmlDocSegment>> documentation)
        {
            _documentation = documentation;
        }

        public TypeDocumentation ParseTypeDocumentation(TypeDefinition typeDefinition)
        {
            var documentation = _documentation[typeDefinition];
            string name = typeDefinition.Name;
            string source = $"{GetTypeModifiers(typeDefinition)} {name} : {typeDefinition.BaseType?.Name}, <interfaces>";

            return new(typeDefinition.Module.Assembly.Name, typeDefinition.Namespace, name)
            {
                Summary = CollectSummary(documentation),
                Source = source,
                Inheritance = new List<string>{"hello", "omegalul"}.ToImmutableArray(),
                TypeParameters = CollectTypeParameters(documentation),
                StaticEvents = typeDefinition.Events.Where(e => e.AddMethod.IsStatic && _documentation.ContainsKey(e)).Select(ParseEventDocumentation).ToImmutableArray(),
                StaticFields = typeDefinition.Fields.Where(f => f.IsStatic && _documentation.ContainsKey(f)).Select(ParseFieldDocumentation).ToImmutableArray(),
                StaticProperties = typeDefinition.Properties.Where(p => p.GetMethod.IsStatic && _documentation.ContainsKey(p)).Select(ParsePropertyDocumentation).ToImmutableArray(),
                StaticMethods = typeDefinition.Methods.Where(m => m.IsStatic && _documentation.ContainsKey(m)).Select(ParseMethodDocumentation).ToImmutableArray(),
                InstanceEvents = typeDefinition.Events.Where(e => !e.AddMethod.IsStatic && _documentation.ContainsKey(e)).Select(ParseEventDocumentation).ToImmutableArray(),
                InstanceFields = typeDefinition.Fields.Where(f => !f.IsStatic && _documentation.ContainsKey(f)).Select(ParseFieldDocumentation).ToImmutableArray(),
                Constructors = typeDefinition.Methods.Where(m => m.IsConstructor && _documentation.ContainsKey(m)).Select(ParseMethodDocumentation).ToImmutableArray(),
                InstanceProperties = typeDefinition.Properties.Where(p => !p.GetMethod.IsStatic && _documentation.ContainsKey(p)).Select(ParsePropertyDocumentation).ToImmutableArray(),
                InstanceMethods = typeDefinition.Methods.Where(m => !m.IsStatic && !m.IsConstructor && _documentation.ContainsKey(m)).Select(ParseMethodDocumentation).ToImmutableArray(),
                Remarks = CollectRemarks(documentation)
            };
        }

        public EventDocumentation ParseEventDocumentation(EventDefinition eventDefinition)
        {
            var documentation = _documentation[eventDefinition];
            string name = eventDefinition.Name;
            string source = $"{GetAccessibilityModifiers(eventDefinition)} {eventDefinition.EventType.Name} {name}";

            return new(name)
            {
                Summary = CollectSummary(documentation),
                Source = source,
                Type = eventDefinition.EventType.Name,
                Remarks = CollectRemarks(documentation)
            };
        }

        public FieldDocumentation ParseFieldDocumentation(FieldDefinition fieldDefinition)
        {
            var documentation = _documentation[fieldDefinition];
            string name = fieldDefinition.Name;
            string type = fieldDefinition.Signature.FieldType.Name;
            string source = $"{GetAccessibilityModifiers(fieldDefinition)} {type} {name}";

            return new(name)
            {
                Summary = CollectSummary(documentation),
                Source = source,
                Type = type,
                Remarks = CollectRemarks(documentation)
            };
        }

        public PropertyDocumentation ParsePropertyDocumentation(PropertyDefinition propertyDefinition)
        {
            var documentation = _documentation[propertyDefinition];
            string name = propertyDefinition.Name;
            string source = $"{GetAccessibilityModifiers(propertyDefinition)} {propertyDefinition.Signature.ReturnType} {name}"
                + " { " + GetPropertyAccessors(propertyDefinition) + " }";

            return new(name)
            {
                Summary = CollectSummary(documentation),
                Source = source,
                Remarks = CollectRemarks(documentation)
            };
        }

        public MethodDocumentation ParseMethodDocumentation(MethodDefinition methodDefinition)
        {
            var documentation = _documentation[methodDefinition];
            string name = methodDefinition.IsConstructor ? methodDefinition.DeclaringType.Name : methodDefinition.Name;
            string type = methodDefinition.IsConstructor ? "" : methodDefinition.Signature.ReturnType.Name + " ";
            string source = $"{GetAccessibilityModifiers(methodDefinition)} {type}{name}(<params>)";

            return new(name)
            {
                Summary = CollectSummary(documentation),
                Source = source,
                TypeParameters = CollectTypeParameters(documentation),
                Parameters = CollectParameters(documentation),
                Remarks = CollectRemarks(documentation)
            };
        }
        
        private static ImmutableArray<IXmlDocSegment> CollectSummary(ImmutableArray<IXmlDocSegment> documentation)
        {
            return documentation.OfType<SummaryXmlDocSegment>().SelectMany(s => s.Content).ToImmutableArray();
        }

        private static ImmutableArray<(string Name, ImmutableArray<IXmlDocSegment> Documentation)>
            CollectTypeParameters(ImmutableArray<IXmlDocSegment> documentation)
        {
            return documentation.OfType<TypeParamXmlDocSegment>().Select(s => (s.TypeParameter, s.Content))
                .ToImmutableArray();
        }

        private static ImmutableArray<(IXmlDocSegment Type, string Name, ImmutableArray<IXmlDocSegment> Documentation)>
            CollectParameters(ImmutableArray<IXmlDocSegment> documentation)
        {
            return documentation.OfType<ParamXmlDocSegment>().Select(s => ((IXmlDocSegment) new RawXmlDocSegment("ass"), s.Parameter, s.Content)).ToImmutableArray();
        }

        private static ImmutableArray<IXmlDocSegment> CollectRemarks(ImmutableArray<IXmlDocSegment> documentation)
        {
            return documentation.OfType<RemarksXmlDocSegment>().SelectMany(s => s.Content).ToImmutableArray();
        }

        private string GuessTypeSignatureSource(TypeSignature signature, IList<CustomAttribute> attributes)
        {
            ExtractNullabilities();
            ExtractDynamic();
            ExtractTupleNames();

            string source = signature.AcceptVisitor(_formatter);
            
            _formatter.ExplicitNullability.Clear();
            _formatter.ExplicitDynamic.Clear();
            _formatter.TupleElementNames.Clear();

            return source;
            
            void ExtractNullabilities()
            {
                if (attributes.Count == 0 || !(attributes.SingleOrDefault(a =>
                    a.Constructor.DeclaringType.Name == "System.Runtime.CompilerServices.NullableAttribute") is { } obj))
                    return;

                var parameter = obj.Signature.FixedArguments[0];
                
                if (parameter.Element is byte b)
                {
                    _formatter.ExplicitNullability.Push((Nullability) b);
                }
                else
                {
                    foreach (var element in parameter.Elements)
                        _formatter.ExplicitNullability.Push((Nullability)(byte) element);
                }
            }

            void ExtractDynamic()
            {
                if (attributes.Count == 0 || !(attributes.SingleOrDefault(a =>
                    a.Constructor.DeclaringType.Name == "System.Runtime.CompilerServices.DynamicAttribute") is { } obj))
                    return;

                var parameter = obj.Signature.FixedArguments[0];
                
                if (parameter.Element is bool b)
                {
                    _formatter.ExplicitDynamic.Push(b);
                }
                else
                {
                    foreach (var element in parameter.Elements)
                        _formatter.ExplicitDynamic.Push((bool) element);
                }
            }

            void ExtractTupleNames()
            {
                if (attributes.Count == 0 || !(attributes.SingleOrDefault(a =>
                        a.Constructor.DeclaringType.Name ==
                        "System.Runtime.CompilerServices.TupleElementNamesAttribute") is
                    { } obj))
                    return;

                var parameter = obj.Signature.FixedArguments[0];
                
                if (parameter.Element is string b)
                {
                    _formatter.TupleElementNames.Push(b);
                }
                else
                {
                    foreach (var element in parameter.Elements)
                        _formatter.TupleElementNames.Push(element as string);
                }
            }
        }
        
        private static string GetAccessibilityModifiers(EventDefinition eventDefinition)
        {
            return GetAccessibilityModifiers(eventDefinition.AddMethod) + " event";
        }
        
        private static string GetAccessibilityModifiers(PropertyDefinition propertyDefinition)
        {
            return GetAccessibilityModifiers(propertyDefinition.GetMethod);
        }

        public static string GetAccessibilityModifiers(FieldDefinition fieldDefinition)
        {
            string accessibility;
            
            if (fieldDefinition.IsPublic)
                accessibility = "public";
            else if (fieldDefinition.IsFamily)
                accessibility = "protected";
            else
                accessibility = "protected internal";

            return $"{accessibility}{(fieldDefinition.IsInitOnly ? " readonly" : "")}";
        }

        private static string GetTypeModifiers(TypeDefinition typeDefinition)
        {
            string type;
            if (typeDefinition.IsEnum)
                type = "enum";
            else if (typeDefinition.IsValueType)
                type = GetStructModifiers(typeDefinition);
            else if (typeDefinition.IsInterface)
                type = "interface";
            else
                type = (typeDefinition.IsAbstract, typeDefinition.IsSealed) switch
                {
                    (true, true) => "static ",
                    (true, false) => "abstract ",
                    (false, true) => "sealed ",
                    _ => ""
                } + (typeDefinition.Methods.SingleOrDefault(m => m.Name == "<Clone>$") is not null ? "record" : "class");

            string visibility = typeDefinition.IsPublic
                ? "public"
                : typeDefinition.IsNestedFamily
                    ? "protected"
                    : "protected internal";

            return $"{visibility} {type}";
        }

        private static string GetStructModifiers(TypeDefinition typeDefinition)
        {
            string readOnly = typeDefinition.IsReadOnly ? "readonly " : "";
            string byRef = typeDefinition.CustomAttributes.SingleOrDefault(c => c.Constructor.DeclaringType.FullName == "System.Runtime.CompilerServices.IsByRefLikeAttribute") is not null
                ? "ref "
                : "";
            return $"{readOnly}{byRef}struct";
        }

        private static string GetAccessibilityModifiers(MethodDefinition methodDefinition)
        {
            if (methodDefinition.IsPublic)
                return "public";
            if (methodDefinition.IsFamilyOrAssembly)
                return "protected internal";
            
            return "protected";
        }

        private static string GetPropertyAccessors(PropertyDefinition propertyDefinition)
        {
            var accessors = new List<string>(2);

            if (propertyDefinition.GetMethod is { })
            {
                accessors.Add("get");
            }

            if (propertyDefinition.SetMethod is { } set)
            {
                if (set.CustomAttributes.SingleOrDefault(c => c.Constructor.DeclaringType.FullName == "System.Runtime.CompilerServices.IsExternalInitAttribute") is null)
                    accessors.Add("set");
                else
                    accessors.Add("init");
            }

            return string.Join("; ", accessors) + ';';
        }
    }
}