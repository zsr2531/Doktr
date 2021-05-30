using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AsmResolver.DotNet;
using Doktr.Dependencies;
using Doktr.Services.GraphTransformers;
using Serilog;

namespace Doktr.Services
{
    public class GraphBuilderService : IGraphBuilderService
    {
        private readonly IAssemblyRepositoryService _repository;
        private readonly IMetadataResolutionService _resolution;
        private readonly IDependencyGraphTransformerProvider _transformerProvider;
        private readonly ILogger _logger;

        public GraphBuilderService(
            IAssemblyRepositoryService repository,
            IMetadataResolutionService resolution,
            IDependencyGraphTransformerProvider transformerProvider,
            ILogger logger)
        {
            _repository = repository;
            _resolution = resolution;
            _transformerProvider = transformerProvider;
            _logger = logger;
        }

        public DependencyGraph BuildGraph()
        {
            var mapping = ImmutableDictionary.CreateBuilder<IFullNameProvider, DependencyNode>();
            var roots = ImmutableArray.CreateBuilder<DependencyNode>();
            var topLevelTypes = new List<TypeDefinition>();
            var context = new GraphBuilderContext(mapping);

            foreach (var assembly in _repository.LoadedAssemblies)
            {
                _logger.Verbose("Processing '{Assembly}'...", assembly.FullName);
                roots.Add(context.GetOrCreateNode(assembly));

                topLevelTypes.AddRange(assembly.ManifestModule.TopLevelTypes);
            }

            foreach (var type in topLevelTypes)
                VisitType(type, context);
            
            foreach (var transformer in _transformerProvider.Transformers)
                context.AcceptTransformer(transformer);
            
            return new DependencyGraph(mapping.ToImmutable(), roots.ToImmutable());
        }

        private void VisitType(TypeDefinition type, GraphBuilderContext context)
        {
            if (context.NodeExists(type))
                return;
            
            _logger.Verbose("Visiting type {Type}...", type.FullName);
            var typeNode = context.GetOrCreateNode(type, GetParent());
            
            VisitTypeMembers(type, context, typeNode);
            VisitTypeAncestors(type, context);

            DependencyNode GetParent()
            {
                if (type.DeclaringType is not { } decl)
                    return context.GetOrCreateNode(type.Module.Assembly);
                if (context.NodeExists(decl))
                    return context.GetOrCreateNode(decl);
                    
                VisitType(decl, context);
                return context.GetOrCreateNode(decl);
            }
        }

        private void VisitTypeMembers(TypeDefinition type, GraphBuilderContext context, DependencyNode typeNode)
        {
            foreach (var @event in type.Events)
                VisitEvent(@event, context, typeNode);

            foreach (var field in type.Fields)
                VisitField(field, context, typeNode);
            
            foreach (var property in type.Properties)
                VisitProperty(property, context, typeNode);

            foreach (var method in type.Methods.Where(m => !context.NodeExists(m)))
                VisitMethod(method, context, typeNode);
            
            foreach (var nestedType in type.NestedTypes)
                VisitType(nestedType, context);
        }

        private void VisitTypeAncestors(TypeDefinition type, GraphBuilderContext context)
        {
            if (_resolution.ResolveType(type.BaseType) is { } baseType)
                VisitType(baseType, context);
            
            foreach (var impl in type.Interfaces)
            {
                var inf = impl.Interface;
                var resolved = _resolution.ResolveType(inf);
                if (resolved is null)
                    continue;
                
                VisitType(resolved, context);
            }
        }

        private void VisitEvent(EventDefinition @event, GraphBuilderContext context, DependencyNode parent)
        {
            _logger.Verbose("Visiting event {Event}...", @event.FullName);
            var node = context.GetOrCreateNode(@event, parent);

            if (@event.AddMethod is { } add)
            {
                _logger.Verbose("Visiting add method {Method} of event {Event}...", add.FullName, @event.FullName);
                context.GetOrCreateNode(add, node);
            }

            if (@event.RemoveMethod is { } remove)
            {
                _logger.Verbose("Visiting remove method {Method} of event {Event}...", remove.FullName, @event.FullName);
                context.GetOrCreateNode(remove, node);
            }

            if (@event.FireMethod is { } fire)
            {
                _logger.Verbose("Visiting fire method {Method} of event {Event}...", fire.FullName, @event.FullName);
                context.GetOrCreateNode(fire, node);
            }
        }

        private void VisitField(FieldDefinition field, GraphBuilderContext context, DependencyNode parent)
        {
            _logger.Verbose("Visiting field {Field}...", field.FullName);
            context.GetOrCreateNode(field, parent);
        }

        private void VisitProperty(PropertyDefinition property, GraphBuilderContext context, DependencyNode parent)
        {
            _logger.Verbose("Visiting property {Property}...", property.FullName);
            var node = context.GetOrCreateNode(property, parent);

            if (property.GetMethod is { } get)
            {
                _logger.Verbose("Visiting getter method {Method} of property {Property}...", get.FullName, property.FullName);
                context.GetOrCreateNode(get, node);
            }

            if (property.SetMethod is { } set)
            {
                _logger.Verbose("Visiting setter method {Method} of property {Property}...", set.FullName, property.FullName);
                context.GetOrCreateNode(set, node);
            }
        }

        private void VisitMethod(MethodDefinition method, GraphBuilderContext context, DependencyNode parent)
        {
            _logger.Verbose("Visiting method {Method}...", method.FullName);
            context.GetOrCreateNode(method, parent);
        }
    }
}