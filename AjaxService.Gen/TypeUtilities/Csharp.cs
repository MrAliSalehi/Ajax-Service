using AjaxService.Gen.Models.Csharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AjaxService.Gen.TypeUtilities;

public static class Csharp
{
    public static T Build<T>(TypeSyntax typeSyntax) where T : CsharpTypeBase, ICsharpTypeConversion<T>, new()
    {
        var output = new CsharpTypeBase();
        FromTypeSyntax(typeSyntax, output);

        return new T().FromBase(output);
    }
    public static bool IsComplex(CsharpTypeBase typeBase)
    {
        if (typeBase.IsCollection)
        {
            return IsComplex(typeBase.Child!);
        }

        return typeBase.ClassModel is not null;
    }
    private static void FromTypeSyntax(TypeSyntax typeSyntax, CsharpTypeBase output)
    {
        switch (typeSyntax)
        {
            case PredefinedTypeSyntax predefinedTypeSyntax:
                IfIsPredefined(output, predefinedTypeSyntax);
                break;
            case IdentifierNameSyntax identifierNameSyntax:
                IfIsCustomClass(output, identifierNameSyntax);
                break;
            case ArrayTypeSyntax arrayTypeSyntax:
                IfIsArray(output, arrayTypeSyntax);
                break;
            case GenericNameSyntax genericNameSyntax:
                IfIsGeneric(output, genericNameSyntax);
                break;
        }
    }
    private static void IfIsPredefined(CsharpTypeBase output, PredefinedTypeSyntax predefinedTypeSyntax)
    {
        output.PredefinedType = PredefinedType.From(predefinedTypeSyntax.Keyword.ValueText);
        output.IsCollection = false;
        output.IsPredefined = true;
    }
    private static void IfIsCustomClass(CsharpTypeBase output, SimpleNameSyntax identifierNameSyntax)
    {
        output.IsCollection = false;
        output.IsPredefined = false;
        CsharpClassModel.GetAndCreateTheClass(identifierNameSyntax.Identifier.ValueText, output);
    }
    private static void IfIsArray(CsharpTypeBase output, ArrayTypeSyntax arrayTypeSyntax)
    {
        output.IsCollection = true;
        output.IsPredefined = false;
        output.NestedLevel = arrayTypeSyntax.RankSpecifiers.Count();
        FromTypeSyntax(arrayTypeSyntax.ElementType, output.Child ??= new());
    }
    private static void IfIsGeneric(CsharpTypeBase output, GenericNameSyntax genericNameSyntax)
    {
        var child = genericNameSyntax.TypeArgumentList.Arguments.Single();
        if (!IsEnumerableType(genericNameSyntax.Identifier.ValueText))
        {
            FromTypeSyntax(child, output);
            return;
        }

        output.IsCollection = true;
        output.IsPredefined = false;
        output.NestedLevel++;
        FromTypeSyntax(child, output.Child ??= new());
    }

    private static bool IsEnumerableType(string type) => Type.GetType($"System.Collections.Generic.{type}`1") is not null;
}