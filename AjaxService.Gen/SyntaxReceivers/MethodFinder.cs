using AjaxService.Gen.Extensions;
using AjaxService.Gen.Models.Csharp;
using AjaxService.Gen.TypeUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

namespace AjaxService.Gen.SyntaxReceivers;

public class MethodFinder : ISyntaxReceiver
{
    public List<CsharpClass> CsharpClasses { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: "Ajax" } } attr)
            return;

        var methodDeclaration = attr.FirstParent<MethodDeclarationSyntax>();

        var className = attr.FirstParent<ClassDeclarationSyntax>().Identifier.Text;

        var functionName = methodDeclaration.Identifier.Text;

        var functionUrl = (attr.ArgumentList!.Arguments.First().Expression as LiteralExpressionSyntax)!.Token.ValueText;

        var csharpClass = CsharpClasses.FirstOrDefault(p => p.Name == className);
        if (csharpClass is null)
        {
            csharpClass = new(className);
            CsharpClasses.Add(csharpClass);
        }

        var csharpFunction = csharpClass.CsharpFunctions.FirstOrDefault(p => p.Name == functionName);
        if (csharpFunction is null)
        {
            csharpFunction = new();
            csharpClass.CsharpFunctions.Add(csharpFunction);
        }
        
        methodDeclaration.AttributeLists.ForEachAttr(p=>
        {
            var isValid = Enum.TryParse(p,out RequestTypeEnum typeEnum);
            if (isValid)
            {
                csharpFunction.RequestType = typeEnum.GetRequestType();
            }
        });
        csharpFunction.Name = functionName;
        csharpFunction.Url = functionUrl;
        csharpFunction.Parameters = GetParameters(methodDeclaration);
        csharpFunction.ReturnType = Csharp.Build<CsharpReturnType>(methodDeclaration.ReturnType);
    }

    private static List<CsharpParameter> GetParameters(MethodDeclarationSyntax methodDeclaration)
    {
        var parameters = methodDeclaration.ParameterList.Parameters.ToList();
        var fromBodyAttrCount = 0;
        var complexTypesCount = 0;

        var list = parameters.Select(parameter => BuildParameter(parameter, ref complexTypesCount, ref fromBodyAttrCount)).ToList();

        if (fromBodyAttrCount > 1)
            throw new Exception($"method:[{methodDeclaration.Identifier.ValueText}] Cannot have more than one [FromBody] in parameters.");

        if (complexTypesCount > 1)
            throw new Exception($"method:[{methodDeclaration.Identifier.ValueText}] Cannot have more than one complex object.");


        return list;
    }
    private static CsharpParameter BuildParameter(ParameterSyntax parameter, ref int complexTypesCount, ref int fromBodyAttrCount)
    {
        var result = Csharp.Build<CsharpParameter>(parameter.Type!);
        result.Name = parameter.Identifier.ValueText;
        parameter.AttributeLists.ForEachAttr(name => { result.ParameterTypeEnum = name.GetParameterType(); });

        if (Csharp.IsComplex(result))
        {
            if (result.ParameterTypeEnum is ParameterTypeEnum.None)
                result.ParameterTypeEnum = ParameterTypeEnum.FromBody;

            complexTypesCount++;
        }
        else
        {
            switch (result.ParameterTypeEnum)
            {
                case ParameterTypeEnum.FromBody when complexTypesCount > 0:
                    throw new Exception("cannot have a complex object and another [FromBody] parameter at the same time");
                case ParameterTypeEnum.FromBody:
                    fromBodyAttrCount++;
                    break;
                case ParameterTypeEnum.None:
                    result.ParameterTypeEnum = ParameterTypeEnum.FromQuery;
                    break;
            }
        }

        return result;
    }
}