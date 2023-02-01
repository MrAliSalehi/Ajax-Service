using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AjaxService.Gen.SyntaxReceivers;

public class ClassDeclarationFinder : ISyntaxReceiver
{
    private static List<ClassDeclarationSyntax> Classes { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
            return;


        if (Classes.Any(p => p.Identifier.ValueText == classDeclaration.Identifier.ValueText))
            return;

        Classes.Add(classDeclaration);
    }
    public static ClassDeclarationSyntax? Get(string className)
    {
        return Classes.FirstOrDefault(p => p.Identifier.ValueText == className);
    }
}