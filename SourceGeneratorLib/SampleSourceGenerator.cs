using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGeneratorLib;

[Generator]
public sealed class SampleSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "ReportAttribute.g.cs", SourceText.From(_attributeSourceCode, Encoding.UTF8)));

        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (syntaxNode, _)       => syntaxNode is ClassDeclarationSyntax,
                transform: (genSyntaxContext, _) => syntaxProviderTransform(genSyntaxContext))
            .Where(cds => cds.hasAttribute)
            .Select((t, _) => t.cds);

        var compilation = context.CompilationProvider.Combine(provider.Collect());

        context.RegisterSourceOutput(compilation, (spc, source) => outputExecute(spc, source.Left, source.Right));
    }

    // Filter classes annotated with the [Report] attribute
    private static (ClassDeclarationSyntax cds, bool hasAttribute) syntaxProviderTransform(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        foreach (AttributeSyntax attributeSyntax in classDeclarationSyntax.AttributeLists.SelectMany(attributeListSyntax => attributeListSyntax.Attributes))
        {
            // attributeSyntax.Name.ToString(); // This is just 'Report'

            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
            {
                continue;
            }

            string attributeName = attributeSymbol.ContainingType.ToDisplayString();

            if (_attributeFullName.Equals(attributeName))
            {
                return (classDeclarationSyntax, true);
            }
        }

        return (classDeclarationSyntax, false);
    }

    private static void outputExecute(SourceProductionContext context, Compilation compilation, IEnumerable<ClassDeclarationSyntax> classDeclarations)
    {
        foreach (ClassDeclarationSyntax classDeclarationSyntax in classDeclarations)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);

            if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
            {
                continue;
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            string className     = classDeclarationSyntax.Identifier.Text;

            IEnumerable<string> methodBody = classSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Select(property =>
                    $$"""
                    yield return $"{{property.Name}} = {this.{{property.Name}}}";
                    """) // yield return $"Id = {this.Id}";
                .Select(x => $"{"",8}{x}"); // PadLeft with 8 spaces

            string code =
                $$"""
                // <auto-generated/>

                using System;
                using System.Collections.Generic;

                namespace {{namespaceName}};

                partial class {{className}}
                {
                    public IEnumerable<string> Report()
                    {
                {{string.Join("\n", methodBody)}}
                    }
                }
                """;

            context.AddSource($"{className}.g.cs", SourceText.From(code, Encoding.UTF8));
        }
    }

    private const string _namespace         = "Generators";
    private const string _attributeName     = "ReportAttribute";
    private const string _attributeFullName = $"{_namespace}.{_attributeName}";

    private const string _attributeSourceCode =
        $$"""
          // <auto-generated/>

          namespace {{_namespace}}
          {
              [System.AttributeUsage(System.AttributeTargets.Class)]
              public class {{_attributeName}} : System.Attribute
              {
              }
          }
          """;
}