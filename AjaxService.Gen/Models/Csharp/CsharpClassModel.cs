using AjaxService.Gen.Extensions;
using AjaxService.Gen.SyntaxReceivers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AjaxService.Gen.Models.Csharp;

public sealed class CsharpClassModel
{
    public string Name { get; set; } = CommonExtensions.GenerateRandomName();
    public List<CsharpProperty> Properties { get; set; } = new();

    public static CsharpClassModel From(ClassDeclarationSyntax classDeclarationSyntax)
    {
        var output = new CsharpClassModel
        {
            Name = classDeclarationSyntax.Identifier.ValueText
        };

        foreach (var prop in GetProperties(classDeclarationSyntax))
            output.Properties.Add(CreateProperty(prop));

        return output;
    }
    public static void GetAndCreateTheClass(string classTypeName, CsharpTypeBase csharpProperty)
    {
        var innerClass = ClassDeclarationFinder.Get(classTypeName);
        if (innerClass is null)
            throw new ArgumentNullException($"the inner class: [{classTypeName}] not found in your codebase");

        csharpProperty.ClassModel = From(innerClass);
    }
    private static CsharpProperty CreateProperty(PropertyDeclarationSyntax prop)
    {
        var result = TypeUtilities.Csharp.Build<CsharpProperty>(prop.Type);

        result.Name = prop.Identifier.ValueText;

        return result;
    }

    private static IEnumerable<PropertyDeclarationSyntax> GetProperties(TypeDeclarationSyntax classDeclarationSyntax) =>
        classDeclarationSyntax.Members
            .Where(member => member is PropertyDeclarationSyntax)
            .Cast<PropertyDeclarationSyntax>();
}