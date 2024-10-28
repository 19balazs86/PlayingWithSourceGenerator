using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGeneratorLib;

public static class Extensions
{
    public static string? GetAttributeFullName(this AttributeSyntax attributeSyntax, GeneratorSyntaxContext context)
    {
        SymbolInfo symbolInfo = context.SemanticModel.GetSymbolInfo(attributeSyntax);

        return symbolInfo is { Symbol: IMethodSymbol attributeSymbol }
            ? attributeSymbol.ContainingType.ToDisplayString()
            : null;
    }

    public static bool HasAttributeWithFullName(this ClassDeclarationSyntax classSyntax, GeneratorSyntaxContext context, string attributeFullName)
    {
        return classSyntax.AttributeLists
            .SelectMany(attributeListSyntax => attributeListSyntax.Attributes)
            .Select(attributeSyntax         => attributeSyntax.GetAttributeFullName(context))
            .Any(attributeFullName.Equals);
    }
}