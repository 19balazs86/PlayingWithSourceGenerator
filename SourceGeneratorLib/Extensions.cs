using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGeneratorLib;

public static class Extensions
{
    public static string? GetAttributeFullName(this AttributeSyntax attributeSyntax, GeneratorSyntaxContext context)
    {
        SymbolInfo symbolInfo = context.SemanticModel.GetSymbolInfo(attributeSyntax);

        return symbolInfo.GetSymbolInfoFullName();
    }

    public static bool HasAttributeWithFullName(this ClassDeclarationSyntax classSyntax, GeneratorSyntaxContext context, string attributeFullName)
    {
        return classSyntax.AttributeLists
            .SelectMany(attributeListSyntax => attributeListSyntax.Attributes)
            .Select(attributeSyntax         => attributeSyntax.GetAttributeFullName(context))
            .Any(attributeFullName.Equals);
    }

    public static string? GetSymbolInfoFullName(this SymbolInfo symbolInfo)
    {
        return symbolInfo is { Symbol: IMethodSymbol attributeSymbol }
            ? attributeSymbol.ContainingType.ToDisplayString()
            : null;
    }

    public static string? GetAttributeFullName(this AttributeSyntax attributeSyntax, Compilation compilation)
    {
        SemanticModel model = compilation.GetSemanticModel(attributeSyntax.SyntaxTree);

        SymbolInfo symbolInfo =  model.GetSymbolInfo(attributeSyntax);

        return symbolInfo.GetSymbolInfoFullName();
    }

    public static string GetFullName(this ClassDeclarationSyntax classDeclaration, Compilation compilation)
    {
        if (compilation.GetSemanticModel(classDeclaration.SyntaxTree)
                       .GetDeclaredSymbol(classDeclaration) is INamedTypeSymbol symbol)
        {
            return $"\"{symbol.ToDisplayString()}\""; // <Namespace>.<ClassName>
        }

        return string.Empty;
    }

    public static string GetFullName(this ClassDeclarationSyntax classDeclaration)
    {
        string ns = GetNamespace(classDeclaration);

        return $"{ns}.{classDeclaration.Identifier.Text}".TrimStart('.');
    }

    public static string GetNamespace(this BaseTypeDeclarationSyntax syntax)
    {
        string @namespace = string.Empty;

        SyntaxNode? potentialNamespaceParent = syntax.Parent;

        while (potentialNamespaceParent is not null and not NamespaceDeclarationSyntax and not FileScopedNamespaceDeclarationSyntax)
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            @namespace = namespaceParent.Name.ToString();

            while (namespaceParent.Parent is NamespaceDeclarationSyntax parent)
            {
                @namespace      = $"{namespaceParent.Name}.{@namespace}";
                namespaceParent = parent;
            }
        }

        return @namespace;
    }
}