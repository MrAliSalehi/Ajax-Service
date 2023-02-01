using Microsoft.CodeAnalysis;

namespace AjaxService.Gen.SyntaxReceivers;

public class MainSyntaxReceiver : ISyntaxReceiver
{
    public ClassDeclarationFinder ClassDeclarationFinder { get; } = new();
    public MethodFinder MethodFinder { get; }= new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        MethodFinder.OnVisitSyntaxNode(syntaxNode);
        ClassDeclarationFinder.OnVisitSyntaxNode(syntaxNode);
    }
}